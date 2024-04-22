// Seleccionar todos los elementos <li> dentro de cualquier elemento con la clase CSS "navigation"
let list = document.querySelectorAll(".navigation li");
 

// CONTENIDO EMPLEADOS ==================================================================================================================================================

function activeLink() {
    // Remover la clase CSS "hovered" de todos los elementos en list
    list.forEach((item) => {
        item.classList.remove("hovered");
    });
    // Agregar la clase CSS "hovered" al elemento actual
    this.classList.add("hovered");
}

// Agregar un evento "mouseover" a cada elemento en list, para ejecutar la función activeLink()
list.forEach((item) => item.addEventListener("mouseover", activeLink));

// Seleccionar el primer elemento que coincide con la clase CSS "toggle"
let toggle = document.querySelector(".toggle");

// Seleccionar el primer elemento que coincide con la clase CSS "navigation"
let navigation = document.querySelector(".navigation");

// Seleccionar el primer elemento que coincide con la clase CSS "main"
let main = document.querySelector(".main");

// Seleccionar el elemento TextNombreUser por su ID
let textBox = document.getElementById("TextNombreUser");

let maquin = document.getElementById("imgclaen");

// Función que se ejecuta cuando se hace clic en toggle
toggle.onclick = function () {
    // Alternar la clase CSS "active" en el elemento navigation
    navigation.classList.toggle("active");

    // Alternar la clase CSS "active" en el elemento main
    main.classList.toggle("active");

    var logo = document.getElementById("logo");
    var mal = document.getElementById("toggle");


     


if (logo.style.display === "none") {
    logo.style.display = "block"; 
    logo.style.height = "auto";
    logo.style.marginTop = "15px";
    logo.style.marginLeft = "33px";
} else {
    logo.style.display = "none";
}

};


