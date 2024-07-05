src = "https://code.jquery.com/jquery-3.6.0.min.js";

document.getElementById("btn_menu").addEventListener("click", mostrar_menu);
document.getElementById("back_menu").addEventListener("click", ocultar_menu);
document.getElementById("close-button").addEventListener("click", ocultar_menu);
nav = document.getElementById("nav");
background_menu = document.getElementById("back_menu");

function mostrar_menu() {
  nav.style.right = "0px";
  background_menu.style.display = "block";
  document.body.style.overflow = "hidden";
}

function ocultar_menu() {
  nav.style.right = "-250px";
  background_menu.style.display = "none";
  document.body.style.overflow = "";
}
