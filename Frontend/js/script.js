$(document).ready(function () {
  "use strict";

  // ✅ دالة تمييز التاسك كمكتملة
  $(document).on("click", ".todo-item input[type='checkbox']", function () {
    $(this).closest('.todo-item').toggleClass('complete');
    filterTasks(); // بعد أي تغيير، نحدّث الفلترة
  });

  // ✅ إضافة مهمة عند الضغط على Enter
  $(".add-task").keypress(function (e) {
    if (e.which == 13 && $(this).val().trim() !== "") {
      addTask($(this).val().trim());
      $(this).val('');
      filterTasks();
    }
  });

  // ✅ إضافة مهمة عند الضغط على زر Add
  $(".add-btn").click(function () {
    let taskText = $(".add-task").val().trim();
    if (taskText.length > 0) {
      addTask(taskText);
      $(".add-task").val('');
      filterTasks();
    } else {
      alert("Please enter a new task");
    }
  });

  // ✅ دالة الإضافة
  function addTask(taskText) {
    let newTask = `
      <div class="todo-item">
        <div class="checker"><input type="checkbox"></div>
        <span>${taskText}</span>
        <a href="javascript:void(0);" class="btn btn-sm btn-danger float-right remove-todo-item ml-2">Remove</a>
      </div>
    `;
    $(".todo-list").append(newTask);
  }

  // ✅ حذف مهمة عند الضغط على زر Remove
  $(document).on("click", ".remove-todo-item", function () {
    $(this).closest('.todo-item').remove();
    filterTasks();
  });

  // ✅ الفلاتر (All / Active / Completed)
  $(".todo-nav .nav-link").click(function (e) {
    e.preventDefault();
    $(".todo-nav .nav-link").removeClass("active");
    $(this).addClass("active");
    filterTasks();
  });

  // ✅ الدالة المسؤولة عن الفلترة الفعلية
  function filterTasks() {
    let filter = $(".todo-nav .nav-link.active").text().trim();

    $(".todo-item").show(); // نبدأ بعرض الكل

    if (filter === "Active") {
      $(".todo-item.complete").hide(); // اخفي المكتملة
    } else if (filter === "Completed") {
      $(".todo-item:not(.complete)").hide(); // اخفي غير المكتملة
    }
  }

  // ✅ فلترة مبدئية
  filterTasks();
});
