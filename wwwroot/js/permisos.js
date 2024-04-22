// Datos de ejemplo
const permisos = [
    { nombre: "Permiso 1", asignadoA: "Usuario 1" },
    { nombre: "Permiso 2", asignadoA: "Usuario 2" },
    { nombre: "Permiso 3", asignadoA: "Usuario 3" },
    { nombre: "Permiso 4", asignadoA: "Usuario 4" },
    { nombre: "Permiso 5", asignadoA: "Usuario 5" },
    { nombre: "Permiso 6", asignadoA: "Usuario 6" },
    { nombre: "Permiso 7", asignadoA: "Usuario 7" },
    { nombre: "Permiso 8", asignadoA: "Usuario 8" },
    { nombre: "Permiso 9", asignadoA: "Usuario 9" },
    { nombre: "Permiso 10", asignadoA: "Usuario 10" },
    { nombre: "Permiso 11", asignadoA: "Usuario 11" },
    { nombre: "Permiso 12", asignadoA: "Usuario 12" },
    { nombre: "Permiso 13", asignadoA: "Usuario 13" },
    { nombre: "Permiso 14", asignadoA: "Usuario 14" }
];

// Función para crear un botón con estilo
function crearBoton(texto, clase) {
    const boton = document.createElement("button");
    boton.textContent = texto;
    boton.classList.add("acciones");
    boton.classList.add(clase);
    return boton;
}

// Función para mostrar los permisos en la tabla
function mostrarPermisos(pagina) {
    const porPagina = 8;
    const inicio = (pagina - 1) * porPagina;
    const fin = inicio + porPagina;
    const cuerpoTabla = document.getElementById("cuerpo-tabla");
    cuerpoTabla.innerHTML = "";
    
    for (let i = inicio; i < fin && i < permisos.length; i++) {
        const permiso = permisos[i];
        const fila = document.createElement("div");
        fila.classList.add("fila-tabla");
        fila.innerHTML = `
            <div class="item-fila">${permiso.nombre}</div>
            <div class="item-fila">${permiso.asignadoA}</div>
            <div class="item-fila acciones">
            </div>
        `;
        
        const acciones = fila.querySelector(".acciones");
        const botonEditar = crearBoton("Editar", "boton-editar");
        const botonEliminar = crearBoton("Eliminar", "boton-eliminar");
        botonEditar.addEventListener("click", () => editar(i));
        botonEliminar.addEventListener("click", () => eliminar(i));
        acciones.appendChild(botonEditar);
        acciones.appendChild(botonEliminar);
        
        cuerpoTabla.appendChild(fila);
    }
}

// Función para mostrar la paginación
function mostrarPaginacion() {
    const total = permisos.length;
    const paginas = Math.ceil(total / 8);
    const paginacion = document.getElementById("paginacion");
    paginacion.innerHTML = "";
    
    for (let i = 1; i <= paginas; i++) {
        const boton = document.createElement("button");
        boton.textContent = i;
        boton.classList.add("boton-paginacion");
        boton.addEventListener("click", () => {
            mostrarPermisos(i);
        });
        paginacion.appendChild(boton);
    }
}

// Función de inicio
function iniciar() {
    mostrarPermisos(1);
    mostrarPaginacion();
}

// Iniciar la aplicación
iniciar();
