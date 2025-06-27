// Toggle submenu visibility
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.has-sub > a').forEach(function (item) {
        item.addEventListener('click', function (e) {
            e.preventDefault();
            let parent = this.parentElement;
            parent.classList.toggle("active");
        });
    });
});
