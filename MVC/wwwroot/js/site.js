// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var dropdownTitle = document.getElementById("dropdown-title");

dropdownTitle.onclick = () => {
  var parent = dropdownTitle.parentElement;

  var dropdownBody = parent.getElementsByClassName("dropdown-body")[0];

  if (dropdownBody.classList.contains("dropdown-show")) {
    dropdownBody.classList.remove("dropdown-show");
  } else {
    dropdownBody.classList.add("dropdown-show");
  }
};
