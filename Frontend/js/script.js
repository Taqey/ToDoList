$(document).ready(function () {
  "use strict";

  // Check authentication first
  if (!authService.isAuthenticated()) {
    window.location.href = 'login.html';
    return;
  }

  let lists = [];
  let allItems = [];
  let currentList = null;
  let editingItem = null;

  // ================== Logout ==================
  $("#logoutBtn").click(function() {
    if (confirm("Are you sure you want to logout?")) {
      authService.logout();
    }
  });

  // ================== Fetch All Items ==================
  function fetchAllItems() {
    apiService.getItems()
      .then(data => {
        allItems = data;
        renderAllItemsList();
      })
      .catch(err => {
        console.error('Error fetching items:', err);
        if (err.message === 'Authentication failed') {
          authService.logout();
        }
      });
  }

  // ================== Render All Items ==================
  function renderAllItemsList() {
    const container = $(".all-items-list");
    container.empty();
    allItems.forEach((item, index) => {
      container.append(`
        <li class="list-group-item d-flex justify-content-between align-items-center" data-index="${index}">
          <span>${item.name || ''} <small>${item.description || ''}</small></span>
          <div class="btn-group btn-group-sm">
            <button class="btn btn-warning edit-item-btn">Edit</button>
            <button class="btn btn-danger delete-item-btn">Delete</button>
          </div>
        </li>
      `);
    });
  }

  // ================== Fetch Lists ==================
  function fetchLists() {
    apiService.getLists()
      .then(data => {
        lists = data;
        renderLists();
      })
      .catch(err => {
        console.error('Error fetching lists:', err);
        if (err.message === 'Authentication failed') {
          authService.logout();
        }
      });
  }

  // ================== Render Lists ==================
  function renderLists() {
    const container = $(".list-of-lists");
    container.empty();
    lists.forEach((list, index) => {
      container.append(`
        <li class="list-group-item" data-index="${index}">
          <span class="list-name">${list.name || ''} - <small>${list.description || ''}</small></span>
          <div class="mt-2">
            <button class="btn btn-sm btn-info edit-list-btn mb-1">Edit List</button>
            <button class="btn btn-sm btn-secondary manage-items-btn mb-1">Manage Items</button>
            <button class="btn btn-sm btn-danger delete-list-btn mb-1">Delete</button>
          </div>
          <div class="edit-list-form border p-2 mt-2 d-none">
            <input type="text" class="form-control mb-2 edit-list-name-input" placeholder="List Name" value="${list.name || ''}">
            <input type="text" class="form-control mb-2 edit-list-desc-input" placeholder="Description" value="${list.description || ''}">
            <button class="btn btn-sm btn-success save-edit-list-btn">Save</button>
            <button class="btn btn-sm btn-secondary cancel-edit-list-btn">Cancel</button>
          </div>
          <div class="items-dropdown border p-2 mt-2 d-none" style="max-height:150px; overflow-y:auto;"></div>
        </li>
      `);
    });
  }

  // ================== Add New List ==================
  $(".show-add-list-btn").click(() => {
    $(".add-list-form").removeClass("d-none");
    $(".show-add-list-btn").addClass("d-none");
  });

  $(".cancel-list-btn").click(() => {
    $(".add-list-name").val('');
    $(".add-list-desc").val('');
    $(".add-list-form").addClass("d-none");
    $(".show-add-list-btn").removeClass("d-none");
  });

  $(".save-list-btn").click(() => {
    const name = $(".add-list-name").val().trim();
    const desc = $(".add-list-desc").val().trim();
    if (!name) return alert("Enter list name");

    apiService.createList(name, desc)
      .then(() => {
        $(".add-list-name").val('');
        $(".add-list-desc").val('');
        $(".add-list-form").addClass("d-none");
        $(".show-add-list-btn").removeClass("d-none");
        fetchLists();
      })
      .catch(err => {
        console.error('Error creating list:', err);
        alert('Failed to create list. Please try again.');
      });
  });

  // ================== Edit List ==================
  $(document).on("click", ".edit-list-btn", function () {
    const li = $(this).closest("li");
    const index = li.data("index");
    const list = lists[index];
    const editForm = li.find(".edit-list-form");
    const itemsDropdown = li.find(".items-dropdown");
    
    // Hide items dropdown if open
    itemsDropdown.addClass("d-none");
    
    // Show edit form
    editForm.removeClass("d-none");
    
    // Pre-fill form with current values
    editForm.find(".edit-list-name-input").val(list.name || '');
    editForm.find(".edit-list-desc-input").val(list.description || '');
  });

  $(document).on("click", ".cancel-edit-list-btn", function () {
    const li = $(this).closest("li");
    li.find(".edit-list-form").addClass("d-none");
  });

  $(document).on("click", ".save-edit-list-btn", function () {
    const li = $(this).closest("li");
    const index = li.data("index");
    const list = lists[index];
    const editForm = li.find(".edit-list-form");
    
    const name = editForm.find(".edit-list-name-input").val().trim();
    const desc = editForm.find(".edit-list-desc-input").val().trim();
    
    if (!name) {
      alert("Enter list name");
      return;
    }

    // Get current items from the list
    const currentItems = list.items || [];
    
    apiService.updateList(list.id, name, desc, currentItems)
      .then(() => {
        // Update local copy
        list.name = name;
        list.description = desc;
        editForm.addClass("d-none");
        renderLists();
      })
      .catch(err => {
        console.error('Error updating list:', err);
        alert('Failed to update list. Please try again.');
      });
  });

  // ================== Manage Items in List ==================
  $(document).on("click", ".manage-items-btn", function () {
    const li = $(this).closest("li");
    const index = li.data("index");
    currentList = lists[index];
    const dropdown = li.find(".items-dropdown");
    const editForm = li.find(".edit-list-form");
    
    // Hide edit form if open
    editForm.addClass("d-none");

    dropdown.empty();
    allItems.forEach(item => {
      const checked = currentList.items && currentList.items.some(i => i.id === item.id) ? "checked" : "";
      dropdown.append(`
        <div>
          <input type="checkbox" class="manage-item-checkbox" data-id="${item.id}" ${checked}> ${item.name || ''}
        </div>
      `);
    });

    dropdown.toggleClass("d-none");

    // Render tasks in main area
    renderTasks();
  });

  // ================== Handle Manage Items Checkbox ==================
  $(document).on("change", ".manage-item-checkbox", function () {
    const itemId = parseInt($(this).data("id"));
    const checked = $(this).is(":checked");

    if (checked) {
      apiService.addItemToList(currentList.id, itemId)
        .then(() => {
          // update local copy
          const addedItem = allItems.find(i => i.id === itemId);
          if (addedItem && !currentList.items.some(i => i.id === itemId)) {
            currentList.items.push(addedItem);
          }
          renderTasks();
        })
        .catch(err => {
          console.error('Error adding item to list:', err);
          $(this).prop('checked', false);
        });
    } else {
      apiService.removeItemFromList(currentList.id, itemId)
        .then(() => {
          currentList.items = currentList.items.filter(i => i.id !== itemId);
          renderTasks();
        })
        .catch(err => {
          console.error('Error removing item from list:', err);
          $(this).prop('checked', true);
        });
    }
  });

  // ================== Render Tasks in Main Area ==================
  function renderTasks() {
    const container = $(".todo-list");
    container.empty();
    if (!currentList || !currentList.items) return;

    currentList.items.forEach(item => {
      container.append(`
        <div class="todo-item ${item.isCompleted ? 'complete' : ''}" data-id="${item.id}">
          <input type="checkbox" class="task-checkbox" ${item.isCompleted ? 'checked' : ''}>
          <span>${item.name || ''} <small>${item.description || ''}</small></span>
          <button class="btn btn-sm btn-danger remove-todo-item">Delete</button>
        </div>
      `);
    });
    filterTasks();
  }

  // ================== Task Checkbox Toggle ==================
  $(document).on("change", ".task-checkbox", function (e) {
    // Stop event propagation to prevent any accidental triggers
    e.stopPropagation();
    
    const itemId = parseInt($(this).closest(".todo-item").data("id"));
    const item = currentList.items.find(i => i.id === itemId);
    
    // Safety check: ensure item exists in current list
    if (!item) {
      console.warn('Item not found in current list:', itemId);
      return;
    }
    
    const isCompleted = $(this).is(":checked");
    const previousState = item.isCompleted;
    
    // CRITICAL: Only update completion status, NEVER remove from list
    // Store reference to ensure item stays in list
    const itemIndex = currentList.items.findIndex(i => i.id === itemId);
    if (itemIndex === -1) {
      console.error('Item not found in list items array');
      $(this).prop('checked', previousState);
      return;
    }
    
    // Update completion status in local state
    item.isCompleted = isCompleted;
    
    // Also update in allItems array to keep it in sync
    const allItem = allItems.find(i => i.id === itemId);
    if (allItem) {
      allItem.isCompleted = isCompleted;
    }

    // Verify item is still in list before rendering
    if (!currentList.items.some(i => i.id === itemId)) {
      console.error('Item was removed from list - restoring it');
      currentList.items.push(item);
    }

    // Re-render to show strike-through change immediately
    renderTasks();

    // Update backend - ONLY the completion status, nothing else
    // IMPORTANT: After updating, we need to ensure the item stays in the list
    // because the backend UpdateItem doesn't preserve ListId, which can break the relationship
    apiService.updateItem(item.id, item.name, item.description, isCompleted)
      .then(() => {
        // After updating item, ensure it's still in the current list
        // The backend might have removed it from the list due to ListId not being preserved
        // So we re-add it to ensure it stays in the list
        return apiService.addItemToList(currentList.id, itemId)
          .then(() => {
            // Verify item is in local state
            if (!currentList.items.some(i => i.id === itemId)) {
              console.warn('Item missing from local state after update - restoring');
              const restoredItem = allItems.find(i => i.id === itemId) || item;
              if (!currentList.items.some(i => i.id === itemId)) {
                currentList.items.push(restoredItem);
                renderTasks();
              }
            }
            console.log('Item completion status updated and ensured in list:', { itemId, isCompleted, listId: currentList.id });
          })
          .catch(addErr => {
            // If addItemToList fails, it might be because item is already in list
            // That's okay - just log it and continue
            console.log('Item might already be in list (or add failed):', addErr);
            // Ensure item is in local state anyway
            if (!currentList.items.some(i => i.id === itemId)) {
              const restoredItem = allItems.find(i => i.id === itemId) || item;
              currentList.items.push(restoredItem);
              renderTasks();
            }
          });
      })
      .catch(err => {
        console.error('Error updating item:', err);
        // Revert checkbox and local state
        $(this).prop('checked', previousState);
        item.isCompleted = previousState;
        if (allItem) {
          allItem.isCompleted = previousState;
        }
        // Ensure item is still in list after error
        if (!currentList.items.some(i => i.id === itemId)) {
          currentList.items.push(item);
        }
        renderTasks();
        alert('Failed to update item. Please try again.');
      });
  });

  // ================== Remove Task ==================
  $(document).on("click", ".remove-todo-item", function () {
    const itemId = parseInt($(this).closest(".todo-item").data("id"));

    if (!currentList) return;

    apiService.removeItemFromList(currentList.id, itemId)
      .then(() => {
        currentList.items = currentList.items.filter(i => i.id !== itemId);
        renderTasks();
      })
      .catch(err => {
        console.error('Error removing item from list:', err);
        alert('Failed to remove item. Please try again.');
      });
  });



  $(document).on("click", ".delete-item-btn", function () {
    const index = $(this).closest("li").data("index");
    const item = allItems[index];

    if (!confirm(`Are you sure you want to delete "${item.name}" permanently?`)) return;

    apiService.deleteItem(item.id)
      .then(() => {
        allItems.splice(index, 1);
        // Remove from all lists locally
        lists.forEach(list => {
          if (list.items) {
            list.items = list.items.filter(i => i.id !== item.id);
          }
        });
        renderAllItemsList();
        renderTasks();
      })
      .catch(err => {
        console.error('Error deleting item:', err);
        alert('Failed to delete item. Please try again.');
      });
  });


  // ================== Tabs Filter ==================
  $(".todo-nav .nav-link").click(function (e) {
    e.preventDefault();
    $(".todo-nav .nav-link").removeClass("active");
    $(this).addClass("active");
    filterTasks();
  });

  function filterTasks() {
    const filter = $(".todo-nav .nav-link.active").text().trim();

    $(".todo-item").each(function() {
      const isComplete = $(this).hasClass("complete");

      if (filter === "All") {
        $(this).show();
      } else if (filter === "Active") {
        // Show only incomplete items (not completed)
        if (isComplete) {
          $(this).hide();
        } else {
          $(this).show();
        }
      } else if (filter === "Completed") {
        // Show only completed items
        if (isComplete) {
          $(this).show();
        } else {
          $(this).hide();
        }
      }
    });
  }



  // ================== Edit & Delete from All Items ==================
  $(document).on("click", ".edit-item-btn", function () {
    const index = $(this).closest("li").data("index");
    editingItem = allItems[index];
    $(".edit-item-name").val(editingItem.name);
    $(".edit-item-desc").val(editingItem.description);
    $("#editItemModal").modal("show");
  });

  $(".save-item-btn").click(function () {
    if (!editingItem) return;
    
    const name = $(".edit-item-name").val().trim();
    const desc = $(".edit-item-desc").val().trim();
    
    apiService.updateItem(editingItem.id, name, desc, editingItem.isCompleted)
      .then(() => {
        editingItem.name = name;
        editingItem.description = desc;
        editingItem = null;
        $("#editItemModal").modal("hide");
        fetchAllItems();
        if (currentList) {
          fetchLists().then(() => renderTasks());
        }
      })
      .catch(err => {
        console.error('Error updating item:', err);
        alert('Failed to update item. Please try again.');
      });
  });

// ================== Show/Hide Add Item Form ==================
$('.show-add-item-btn').click(function() {
  $('.add-item-form').toggleClass('d-none'); // يظهر أو يخفي الفورم
});

$('.cancel-item-btn').click(function() {
  $('.add-item-form').addClass('d-none'); // يخفي الفورم
  $('.add-task').val(''); // يمسح الاسم
  $('.add-task-desc').val(''); // يمسح الوصف
});

  // ================== Save New Item ==================
  $('.save-new-item-btn').click(function() {
    const name = $('.add-task').val().trim();
    const desc = $('.add-task-desc').val().trim();
    if (!name) return alert("Enter item name");

    apiService.createItem(name, desc, false)
      .then(newItem => {
        allItems.push(newItem);
        renderAllItemsList();
        $('.add-task').val('');
        $('.add-task-desc').val('');
        $('.add-item-form').addClass('d-none');
      })
      .catch(err => {
        console.error('Error creating item:', err);
        alert('Failed to create item. Please try again.');
      });
  });
  // ================== Delete List ==================
  $(document).on("click", ".delete-list-btn", function() {
    const li = $(this).closest("li");
    const index = li.data("index");
    const list = lists[index];

    if (!confirm(`Are you sure you want to delete the list "${list.name}"?`)) return;

    apiService.deleteList(list.id)
      .then(() => {
        lists.splice(index, 1);
        renderLists();
        // If current list is deleted, clear tasks area
        if (currentList && currentList.id === list.id) {
          currentList = null;
          $(".todo-list").empty();
        }
      })
      .catch(err => {
        console.error('Error deleting list:', err);
        alert('Failed to delete list. Please try again.');
      });
  });

  // ================== Initial Fetch ==================
  fetchAllItems();
  fetchLists();
});
