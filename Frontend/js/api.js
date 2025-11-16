// API Service - handles authenticated API calls with automatic token refresh

class ApiService {
  constructor() {
    this.baseUrl = 'https://localhost:7131/api';
  }

  // Make authenticated request with automatic token refresh
  async authenticatedFetch(url, options = {}) {
    // Ensure we have credentials for cookies
    options.credentials = options.credentials || 'include';
    
    try {
      // CRITICAL: Check token expiry and refresh BEFORE making the request
      // This ensures we always have a valid token before sending API requests
      const authHeader = await authService.getAuthHeader();
      if (authHeader) {
        options.headers = options.headers || {};
        options.headers['Authorization'] = authHeader;
      } else {
        // No token available - user needs to login
        throw new Error('No authentication token available');
      }

      // Make the request with valid token
      let response = await fetch(url, options);
      
      // If still unauthorized (shouldn't happen if token was refreshed, but handle it)
      if (response.status === 401) {
        try {
          // Try one more refresh attempt
          await authService.refreshToken();
          const newAuthHeader = await authService.getAuthHeader();
          if (newAuthHeader) {
            options.headers = options.headers || {};
            options.headers['Authorization'] = newAuthHeader;
            response = await fetch(url, options);
          } else {
            throw new Error('Token refresh failed');
          }
        } catch (refreshError) {
          // Refresh failed - logout and redirect
          authService.logout();
          throw new Error('Authentication failed');
        }
      }

      return response;
    } catch (error) {
      // If it's an auth error, logout has already been handled
      if (error.message.includes('token') || error.message.includes('Authentication')) {
        throw error;
      }
      // Re-throw other errors
      throw error;
    }
  }

  // Lists API
  async getLists() {
    const response = await this.authenticatedFetch(`${this.baseUrl}/Lists`);
    if (!response.ok) throw new Error('Failed to fetch lists');
    return await response.json();
  }

  async getList(id) {
    const response = await this.authenticatedFetch(`${this.baseUrl}/Lists/${id}`);
    if (!response.ok) throw new Error('Failed to fetch list');
    return await response.json();
  }

  async createList(name, description) {
    const response = await this.authenticatedFetch(`${this.baseUrl}/Lists`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name, description })
    });
    if (!response.ok) throw new Error('Failed to create list');
    // Backend returns void (Task), not JSON
    return null;
  }

  async updateList(id, name, description, items) {
    const response = await this.authenticatedFetch(`${this.baseUrl}/Lists/EditList/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name, description, items: items || [] })
    });
    if (!response.ok) throw new Error('Failed to update list');
  }

  async addItemToList(listId, itemId) {
    const response = await this.authenticatedFetch(`${this.baseUrl}/Lists/AddItem/${listId}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(itemId)
    });
    if (!response.ok) throw new Error('Failed to add item to list');
  }

  async removeItemFromList(listId, itemId) {
    const response = await this.authenticatedFetch(`${this.baseUrl}/Lists/RemoveItem/${listId}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(itemId)
    });
    if (!response.ok) throw new Error('Failed to remove item from list');
  }

  async deleteList(id) {
    const response = await this.authenticatedFetch(`${this.baseUrl}/Lists/${id}`, {
      method: 'DELETE'
    });
    if (!response.ok) throw new Error('Failed to delete list');
  }

  // Items API
  async getItems() {
    const response = await this.authenticatedFetch(`${this.baseUrl}/Items`);
    if (!response.ok) throw new Error('Failed to fetch items');
    return await response.json();
  }

  async getItem(id) {
    const response = await this.authenticatedFetch(`${this.baseUrl}/Items/${id}`);
    if (!response.ok) throw new Error('Failed to fetch item');
    return await response.json();
  }

  async createItem(name, description, isCompleted = false) {
    const response = await this.authenticatedFetch(`${this.baseUrl}/Items`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name, description, isCompleted })
    });
    if (!response.ok) throw new Error('Failed to create item');
    return await response.json();
  }

  async updateItem(id, name, description, isCompleted) {
    const response = await this.authenticatedFetch(`${this.baseUrl}/Items/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name, description, isCompleted })
    });
    if (!response.ok) throw new Error('Failed to update item');
    return await response.json();
  }

  async deleteItem(id) {
    const response = await this.authenticatedFetch(`${this.baseUrl}/Items/${id}`, {
      method: 'DELETE'
    });
    if (!response.ok) throw new Error('Failed to delete item');
  }
}

// Create singleton instance
const apiService = new ApiService();

