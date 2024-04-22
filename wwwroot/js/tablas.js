document.addEventListener("DOMContentLoaded", function() {
    const tableBody = document.getElementById("tableBody");
    const pagination = document.getElementById("pageNumbers");
    const prevPageBtn = document.getElementById("prevPageBtn");
    const nextPageBtn = document.getElementById("nextPageBtn");
  
    const data = [
      {id: 1, usuario: "Usuario1", rol: "Rol1", estado: "Activo", fechaCreacion: "2022-04-20" },
      {id: 2, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 3, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 4, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 5, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 6, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 7, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 8, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 9, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 10, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 12, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 13, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 14, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 15, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 16, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 17, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      {id: 18, usuario: "Usuario2", rol: "Rol2", estado: "Inactivo", fechaCreacion: "2022-04-21" },
      // Añade más datos según sea necesario
    ];
  
    const itemsPerPage = 8;
    let currentPage = 1;
  
    function displayData() {
        tableBody.innerHTML = "";
        const startIndex = (currentPage - 1) * itemsPerPage;
        const endIndex = startIndex + itemsPerPage;
        const paginatedData = data.slice(startIndex, endIndex);
      
        paginatedData.forEach(function(item) {
          const row = document.createElement("tr");
          row.innerHTML = `
            <td>${item.usuario}</td>
            <td>${item.rol}</td>
            <td>${item.estado}</td>
            <td>${item.fechaCreacion}</td>
            <td>
            
              <a href="@Url.Action("editarUsuario", new { id = ${item.id })" class="edit-btn">Editar</a>
              <button class="delete-btn">Eliminar</button>
            </td>
          `;
          tableBody.appendChild(row);
        });
      
        displayPagination();
      }
      
  
    function displayPagination() {
      pagination.innerHTML = "";
      const pageCount = Math.ceil(data.length / itemsPerPage);
      for (let i = 1; i <= pageCount; i++) {
        const pageBtn = document.createElement("button");
        pageBtn.textContent = i;
        pageBtn.classList.add("page-btn");
        if (i === currentPage) {
          pageBtn.classList.add("active");
        }
        pagination.appendChild(pageBtn);
      }
    }
  
    function updateTable() {
      document.querySelectorAll(".page-btn").forEach(btn => btn.classList.remove("active"));
      displayData();
    }
  
    prevPageBtn.addEventListener("click", function() {
      if (currentPage > 1) {
        currentPage--;
        updateTable();
      }
    });
  
    nextPageBtn.addEventListener("click", function() {
      if (currentPage < Math.ceil(data.length / itemsPerPage)) {
        currentPage++;
        updateTable();
      }
    });
  
    pagination.addEventListener("click", function(e) {
      if (e.target.classList.contains("page-btn")) {
        currentPage = parseInt(e.target.textContent);
        updateTable();
      }
    });
  
    displayData();
  });
  

  // Obtener la modal
const modal = document.getElementById("newUserModal");

// Obtener el botón que abre la modal
const newUserBtn = document.getElementById("newUserBtn");

// Obtener el elemento span que cierra la modal
const closeBtn = document.querySelector(".close-btn");

// Cuando el usuario hace clic en el botón, abrir la modal
newUserBtn.addEventListener("click", function() {
  modal.style.display = "block";
});

// Cuando el usuario hace clic en la x, cerrar la modal
closeBtn.addEventListener("click", function() {
  modal.style.display = "none";
});

// Cuando el usuario hace clic fuera de la modal, cerrarla
window.addEventListener("click", function(event) {
  if (event.target == modal) {
    modal.style.display = "none";
  }
});
