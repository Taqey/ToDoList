$(document).ready(function () {
  "use strict";

  let lists = [];
  let currentList = null;
  let editingItem = null;

  // ================== Lists ==================
  function renderLists() {
    let listContainer = $(".list-of-lists");
    listContainer.empty();
    lists.forEach((list, index) => {
      listContainer.append(`
        <li class="list-group-item" data-index="${index}">
          <span>${list.name} - <small>${list.description}</small></span>
          <div>
            <button class="btn btn-sm btn-info edit-list-btn">Edit List</button>
            <button class="btn btn-sm btn-secondary manage-items-btn">Manage Items</button>
            <button class="btn btn-sm btn-danger delete-list-btn">Delete</button>
          </div>
        </li>
      `);
    });
  }

  // Show Add List Form
  $(".show-add-list-btn").click(function () {
    $(".add-list-form").removeClass("d-none");
    $(this).addClass("d-none");
  });

  // Save New List
  $(".save-list-btn").click(function () {
    let name = $(".add-list-name").val().trim();
    let desc = $(".add-list-desc").val().trim();
    if (!name) return alert("Enter list name");
    lists.push({name, description: desc, items: []});
    $(".add-list-name").val('');
    $(".add-list-desc").val('');
    $(".add-list-form").addClass("d-none");
    $(".show-add-list-btn").removeClass("d-none");
    renderLists();
  });

  // Cancel Add List
  $(".cancel-list-btn").click(function () {
    $(".add-list-name").val('');
    $(".add-list-desc").val('');
    $(".add-list-form").addClass("d-none");
    $(".show-add-list-btn").removeClass("d-none");
  });

  // Manage Items
  $(document).on("click", ".manage-items-btn", function () {
    let index = $(this).closest("li").data("index");
    currentList = lists[index];
    renderTasks();
  });

  // Edit List
  $(document).on("click", ".edit-list-btn", function () {
    let index = $(this).closest("li").data("index");
    let list = lists[index];
    let formHtml = `
      <div class="edit-list-form mb-2">
        <input type="text" class="form-control mb-1 edit-list-name" value="${list.name}">
        <input type="text" class="form-control mb-1 edit-list-desc" value="${list.description}">
        <button class="btn btn-success btn-sm save-edit-list-btn">Save</button>
        <button class="btn btn-secondary btn-sm cancel-edit-list-btn">Cancel</button>
      </div>
    `;
    $(this).closest("li").append(formHtml);
    $(this).hide();
  });

  $(document).on("click", ".save-edit-list-btn", function () {
    let li = $(this).closest("li");
    let index = li.data("index");
    let list = lists[index];
    list.name = li.find(".edit-list-name").val().trim();
    list.description = li.find(".edit-list-desc").val().trim();
    li.find(".edit-list-form").remove();
    li.find(".edit-list-btn").show();
    renderLists();
  });

  $(document).on("click", ".cancel-edit-list-btn", function () {
    let li = $(this).closest("li");
    li.find(".edit-list-form").remove();
    li.find(".edit-list-btn").show();
  });

  // Delete List
  $(document).on("click", ".delete-list-btn", function () {
    let index = $(this).closest("li").data("index");
    lists.splice(index, 1);
    currentList = null;
    renderLists();
    renderTasks();
  });

  // ================== Tasks ==================
  function renderTasks() {
    let taskContainer = $(".todo-list");
    taskContainer.empty();
    if (!currentList) return;
    currentList.items.forEach((item, index) => {
      taskContainer.append(`
        <div class="todo-item ${item.complete ? 'complete' : ''}" data-index="${index}">
          <div class="checker"><input type="checkbox" ${item.complete ? 'checked' : ''}></div>
          <span>${item.name} <small>${item.description}</small></span>
          <div>
            <button class="btn btn-sm btn-warning edit-todo-item">Edit</button>
            <button class="btn btn-sm btn-danger remove-todo-item">Remove</button>
          </div>
        </div>
      `);
    });
    filterTasks();
  }

  // Add Task
  $(".add-btn").click(function () {
    let name = $(".add-task").val().trim();
    let desc = $(".add-task-desc").val().trim();
    if (!currentList) return alert("Select a list first");
    if (!name) return alert("Enter task name");
    currentList.items.push({name, description: desc, complete: false});
    $(".add-task").val('');
    $(".add-task-desc").val('');
    renderTasks();
  });

  // Toggle Complete
  $(document).on("click", ".todo-item input[type='checkbox']", function () {
    let index = $(this).closest(".todo-item").data("index");
    let item = currentList.items[index];
    item.complete = !item.complete;
    renderTasks();
  });

  // Remove Task
  $(document).on("click", ".remove-todo-item", function () {
    let index = $(this).closest(".todo-item").data("index");
    currentList.items.splice(index, 1);
    renderTasks();
  });

  // Edit Task
  $(document).on("click", ".edit-todo-item", function () {
    let index = $(this).closest(".todo-item").data("index");
    editingItem = currentList.items[index];
    $(".edit-item-name").val(editingItem.name);
    $(".edit-item-desc").val(editingItem.description);
    $("#editItemModal").modal("show");
  });

  $(".save-item-btn").click(function () {
    editingItem.name = $(".edit-item-name").val().trim();
    editingItem.description = $(".edit-item-desc").val().trim();
    editingItem = null;
    $("#editItemModal").modal("hide");
    renderTasks();
  });

  // Nav filters
  $(".todo-nav .nav-link").click(function (e) {
    e.preventDefault();
    $(".todo-nav .nav-link").removeClass("active");
    $(this).addClass("active");
    filterTasks();
  });

  function filterTasks() {
    let filter = $(".todo-nav .nav-link.active").text().trim();
    if (!currentList) return;
    $(".todo-item").show();
    if (filter === "Active") $(".todo-item.complete").hide();
    if (filter === "Completed") $(".todo-item:not(.complete)").hide();
  }

  // Enter key adds Task
  $(".add-task").keypress(function (e) {
    if (e.which === 13) $(".add-btn").click();
  });
});
