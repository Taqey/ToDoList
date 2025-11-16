// Authentication Service - handles login, registration, token storage, and refresh

class AuthService {
  constructor() {
    this.baseUrl = 'https://localhost:7131/api';
    this.tokenKey = 'jwt_token';
    this.tokenExpiryKey = 'jwt_token_expiry';
    this.storageTypeKey = 'token_storage_type'; // 'localStorage' or 'sessionStorage'
    this.useLocalStorage = true; // Default to localStorage
    this.refreshInProgress = false; // Prevent multiple simultaneous refresh attempts
    this.initTokenRefresh();
  }

  // Get storage based on "Remember Me" setting
  getStorage() {
    const storageType = localStorage.getItem(this.storageTypeKey) || 'localStorage';
    return storageType === 'localStorage' ? localStorage : sessionStorage;
  }

  // Store token and expiry with storage type preference
  setToken(token, expiresOn, rememberMe = true) {
    this.useLocalStorage = rememberMe;
    const storage = rememberMe ? localStorage : sessionStorage;
    
    // Store in chosen storage
    storage.setItem(this.tokenKey, token);
    if (expiresOn) {
      const expiryDate = new Date(expiresOn);
      storage.setItem(this.tokenExpiryKey, expiryDate.toISOString());
    }
    
    // Store preference in localStorage so we know which storage to check
    localStorage.setItem(this.storageTypeKey, rememberMe ? 'localStorage' : 'sessionStorage');
    
    // Clear from the other storage to avoid conflicts
    const otherStorage = rememberMe ? sessionStorage : localStorage;
    otherStorage.removeItem(this.tokenKey);
    otherStorage.removeItem(this.tokenExpiryKey);
    
  }

  // Get stored token from appropriate storage
  getToken() {
    const storage = this.getStorage();
    return storage.getItem(this.tokenKey);
  }

  // Get token expiry from appropriate storage
  getTokenExpiry() {
    const storage = this.getStorage();
    return storage.getItem(this.tokenExpiryKey);
  }

  // Check if token is expired (with buffer time)
  isTokenExpired() {
    const expiryStr = this.getTokenExpiry();
    if (!expiryStr) return true;
    
    const expiryDate = new Date(expiryStr);
    const now = new Date();
    // Refresh 5 minutes before expiry to be safe
    const bufferTime = 5 * 60 * 1000; // 5 minutes in milliseconds
    
    return now.getTime() >= (expiryDate.getTime() - bufferTime);
  }

  // Clear token from both storages
  clearToken() {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.tokenExpiryKey);
    sessionStorage.removeItem(this.tokenKey);
    sessionStorage.removeItem(this.tokenExpiryKey);
    localStorage.removeItem(this.storageTypeKey);
    
    // Clear refresh token cookie (if accessible)
    // Note: HttpOnly cookies can't be deleted from JavaScript, but we try to expire it
    this.clearRefreshTokenCookie();
  }

  // Clear refresh token cookie
  clearRefreshTokenCookie() {
    // Try to clear the RefreshToken cookie by setting it to expire
    // Since it's HttpOnly, we can't directly delete it, but we can try to overwrite it
    document.cookie = 'RefreshToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/; SameSite=None; Secure;';
    document.cookie = 'RefreshToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
  }

  // Check if user is authenticated (has valid token)
  isAuthenticated() {
    const token = this.getToken();
    return !!token; // Return true if token exists, expiry check happens before requests
  }

  // Ensure valid token before API request (checks expiry and refreshes if needed)
  async ensureValidToken() {
    const token = this.getToken();
    if (!token) {
      throw new Error('No token available');
    }

    // If token is expired or about to expire, refresh it
    if (this.isTokenExpired()) {
      // Prevent multiple simultaneous refresh attempts
      if (this.refreshInProgress) {
        // Wait for ongoing refresh to complete
        let attempts = 0;
        while (this.refreshInProgress && attempts < 50) {
          await new Promise(resolve => setTimeout(resolve, 100));
          attempts++;
        }
        // Check if we have a valid token now
        if (!this.isTokenExpired()) {
          return this.getToken();
        }
      }

      this.refreshInProgress = true;
      try {
        await this.refreshToken();
        this.refreshInProgress = false;
        return this.getToken();
      } catch (error) {
        this.refreshInProgress = false;
        // Refresh failed - clear tokens and throw error
        this.clearToken();
        throw new Error('Token refresh failed');
      }
    }

    return token;
  }

  // Login with optional "Remember Me"
  async login(username, password, rememberMe = true) {
    const formData = new FormData();
    formData.append('Username', username);
    formData.append('password', password);

    try {
      const response = await fetch(`${this.baseUrl}/Users/Login`, {
        method: 'POST',
        body: formData,
        credentials: 'include' // Important for cookies (refresh token)
      });

      if (!response.ok) {
        // Backend returns BadRequest with a string message, not JSON
        let errorMessage = 'Login failed. Please check your credentials.';
        try {
          const contentType = response.headers.get('content-type');
          if (contentType && contentType.includes('application/json')) {
            const error = await response.json();
            errorMessage = error.message || error.Message || errorMessage;
          } else {
            // Try to get text response
            const text = await response.text();
            if (text) {
              errorMessage = text;
            }
          }
        } catch (parseError) {
          // If parsing fails, use default message
          console.error('Error parsing error response:', parseError);
        }
        throw new Error(errorMessage);
      }

      const data = await response.json();
      
      if (data.token && data.expiresOn) {
        this.setToken(data.token, data.expiresOn, rememberMe);
        return { success: true, token: data.token, expiresOn: data.expiresOn };
      } else {
        throw new Error('Invalid response from server');
      }
    } catch (error) {
      console.error('Login error:', error);
      // Re-throw with proper error message
      if (error.message) {
        throw error;
      }
      throw new Error('Login failed. Please try again.');
    }
  }

  // Register with optional "Remember Me"
  async register(userData, rememberMe = true) {
    const formData = new FormData();
    formData.append('FirstName', userData.firstName);
    formData.append('LastName', userData.lastName);
    formData.append('Password', userData.password);
    formData.append('PhoneNumber', userData.phoneNumber);
    formData.append('Email', userData.email);
    formData.append('UserName', userData.userName);

    try {
      const response = await fetch(`${this.baseUrl}/Users/Register`, {
        method: 'POST',
        body: formData,
        credentials: 'include'
      });

      if (!response.ok) {
        const error = await response.text();
        throw new Error(error || 'Registration failed');
      }

      const message = await response.text();
      
      // After successful registration, automatically log in to get token
      if (message.includes('Successfuly')) {
        try {
          const loginResult = await this.login(userData.userName, userData.password, rememberMe);
          return { success: true, message, ...loginResult };
        } catch (loginError) {
          // Registration succeeded but auto-login failed
          return { success: true, message: 'Registration successful. Please log in.', autoLoginFailed: true };
        }
      } else {
        throw new Error(message);
      }
    } catch (error) {
      console.error('Registration error:', error);
      throw error;
    }
  }

  // Refresh token
  async refreshToken() {
    try {
      const response = await fetch(`${this.baseUrl}/Users/Refresh`, {
        method: 'POST',
        credentials: 'include' // Important for sending cookies
      });

      if (!response.ok) {
        throw new Error('Token refresh failed');
      }

      const data = await response.json();
      
      if (data.token && data.expiresOn) {
        // Preserve the storage type preference when refreshing
        const storageType = localStorage.getItem(this.storageTypeKey) || 'localStorage';
        const rememberMe = storageType === 'localStorage';
        this.setToken(data.token, data.expiresOn, rememberMe);
        return { success: true, token: data.token, expiresOn: data.expiresOn };
      } else {
        throw new Error('Invalid refresh response');
      }
    } catch (error) {
      console.error('Token refresh error:', error);
      this.clearToken();
      throw error;
    }
  }

  // Initialize automatic token refresh (background check)
  initTokenRefresh() {
    // Check token expiry every minute (background monitoring)
    setInterval(() => {
      if (this.isAuthenticated() && this.isTokenExpired() && !this.refreshInProgress) {
        this.refreshToken().catch(err => {
          console.error('Automatic token refresh failed:', err);
          // Only redirect if we're not already on login/register pages
          const currentPath = window.location.pathname;
          if (!currentPath.includes('login.html') && !currentPath.includes('register.html')) {
            this.clearToken();
            window.location.href = 'login.html';
          }
        });
      }
    }, 60000); // Check every minute
  }

  // Logout - clears all authentication data and redirects
  async logout() {
    // Clear all authentication data (tokens, cookies, and any other auth-related data)
    this.clearAllAuthData();
    
    // Try to call backend logout endpoint if it exists (optional - for server-side cleanup)
    // Note: This is optional since we're clearing everything client-side
    try {
      // Attempt to notify backend (if logout endpoint exists)
      await fetch(`${this.baseUrl}/Users/Logout`, {
        method: 'POST',
        credentials: 'include'
      }).catch(() => {
        // Ignore errors - backend might not have logout endpoint
      });
    } catch (err) {
      // Ignore - backend logout is optional
    }
    
    // Redirect to login, preventing access to protected pages
    if (window.location.pathname !== '/login.html' && window.location.pathname !== '/register.html') {
      window.location.href = 'login.html';
    }
  }

  // Clear all authentication-related data
// Clear all authentication-related data
clearAllAuthData() {
    // Clear tokens from both storages
    const keysToRemove = [
        this.tokenKey,
        this.tokenExpiryKey,
        this.storageTypeKey,
        'auth_token',
        'access_token',
        'refresh_token',
        'user_data',
        'user_info',
        'storePromptShown',
        'Auth'
    ];

    keysToRemove.forEach(key => {
        localStorage.removeItem(key);
        sessionStorage.removeItem(key);
    });

    // Clear refresh token cookie
    this.clearRefreshTokenCookie();
}

  // Get Authorization header (with automatic token refresh)
  async getAuthHeader() {
    try {
      const token = await this.ensureValidToken();
      return token ? `Bearer ${token}` : null;
    } catch (error) {
      // If token refresh fails, logout and redirect
      if (window.location.pathname !== '/login.html' && window.location.pathname !== '/register.html') {
        this.logout();
      }
      throw error;
    }
  }
}

// Create singleton instance
const authService = new AuthService();

