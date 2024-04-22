document.addEventListener("DOMContentLoaded", function() {
  const tableBody = document.querySelector(".players-table tbody");
  const paginationMenu = document.querySelector(".pagination-menu");

  const playersData = [
    { foto: "https://via.placeholder.com/50", nombre: "John Doe", edad: 25, numero: "123456789" },
    { foto: "https://via.placeholder.com/50", nombre: "Jane Doe", edad: 30, numero: "987654321" },
    { foto: "https://via.placeholder.com/50", nombre: "Jane Doe", edad: 30, numero: "987654321" },
    { foto: "https://via.placeholder.com/50", nombre: "Jane Doe", edad: 30, numero: "987654321" },
    { foto: "https://via.placeholder.com/50", nombre: "Jane Doe", edad: 30, numero: "987654321" },
    { foto: "https://via.placeholder.com/50", nombre: "Jane Doe", edad: 30, numero: "987654321" },
    { foto: "https://via.placeholder.com/50", nombre: "Jane Doe", edad: 30, numero: "987654321" },
    { foto: "https://via.placeholder.com/50", nombre: "Jane Doe", edad: 30, numero: "987654321" },
    { foto: "https://via.placeholder.com/50", nombre: "Jane Doe", edad: 30, numero: "987654321" },
    { foto: "https://via.placeholder.com/50", nombre: "Jane Doe", edad: 30, numero: "987654321" },
    { foto: "https://via.placeholder.com/50", nombre: "Jane Doe", edad: 30, numero: "987654321" },
    { foto: "https://via.placeholder.com/50", nombre: "Jane Doe", edad: 30, numero: "987654321" },
    { foto: "https://via.placeholder.com/50", nombre: "Jane Doe", edad: 30, numero: "987654321" },
    { foto: "https://via.placeholder.com/50", nombre: "Jane Doe", edad: 30, numero: "987654321" },
    { foto: "https://via.placeholder.com/50", nombre: "Jane Doe", edad: 30, numero: "987654321" },

    // Agrega más datos de jugadores aquí según sea necesario
  ];

  const itemsPerPage = 10;
  let currentPage = 1;

  generateTable(tableBody, playersData, currentPage);
  generatePagination(currentPage, playersData);

  function generateTable(tableBody, playersData, currentPage) {
    tableBody.innerHTML = ""; // Limpiar tabla
    const start = (currentPage - 1) * itemsPerPage;
    const end = start + itemsPerPage;
    const pageData = playersData.slice(start, end);
    pageData.forEach(player => {
      const newRow = `
        <tr>
          <td><img src="${player.foto}" alt="Avatar"></td>
          <td>${player.nombre}</td>
          <td>${player.edad}</td>
          <td>${player.numero}</td>
          <td>
            <button class="edit-btn">Editar</button>
            <button class="delete-btn">Eliminar</button>
          </td>
        </tr>
      `;
      tableBody.innerHTML += newRow;
    });
  }

  function generatePagination(currentPage, playersData) {
    paginationMenu.innerHTML = ""; // Limpiar paginación
    const totalPages = Math.ceil(playersData.length / itemsPerPage);
    for (let i = 1; i <= totalPages; i++) {
      const pageItem = document.createElement("span");
      pageItem.textContent = i;
      pageItem.classList.add("pagination-item");
      if (i === currentPage) {
        pageItem.classList.add("active");
      }
      pageItem.addEventListener("click", function() {
        currentPage = i;
        generateTable(tableBody, playersData, currentPage);
        updateActivePage();
      });
      paginationMenu.appendChild(pageItem);
    }
  }

  function updateActivePage() {
    const pageItems = paginationMenu.querySelectorAll(".pagination-item");
    pageItems.forEach(item => item.classList.remove("active"));
    pageItems[currentPage - 1].classList.add("active");
  }
});

//PARTIDO 
document.addEventListener("DOMContentLoaded", function() {
  // Función para generar las filas de partidos pendientes
  function generatePendingMatchesTable(currentPage) {
    const tableBody = document.querySelector("#pending-matches tbody");
    const paginationMenu = document.querySelector("#pending-matches + .pagination .pagination-menu");

    // Datos de los partidos pendientes
    const pendingMatchesData = [
      { partido: "Partido 1", fecha: "2024-04-21", hora: "15:00", locacion: "Estadio A" },
      { partido: "Partido 2", fecha: "2024-04-22", hora: "16:00", locacion: "Estadio B" },
      { partido: "Partido 2", fecha: "2024-04-22", hora: "16:00", locacion: "Estadio B" },
      { partido: "Partido 2", fecha: "2024-04-22", hora: "16:00", locacion: "Estadio B" },
      { partido: "Partido 2", fecha: "2024-04-22", hora: "16:00", locacion: "Estadio B" },
      { partido: "Partido 2", fecha: "2024-04-22", hora: "16:00", locacion: "Estadio B" },
      { partido: "Partido 2", fecha: "2024-04-22", hora: "16:00", locacion: "Estadio B" },
      { partido: "Partido 2", fecha: "2024-04-22", hora: "16:00", locacion: "Estadio B" },
      { partido: "Partido 2", fecha: "2024-04-22", hora: "16:00", locacion: "Estadio B" },
      { partido: "Partido 2", fecha: "2024-04-22", hora: "16:00", locacion: "Estadio B" },
      { partido: "Partido 2", fecha: "2024-04-22", hora: "16:00", locacion: "Estadio B" },
      // Agrega más datos de partidos pendientes aquí según sea necesario
    ];

    const itemsPerPage = 10;

    // Función para generar la tabla de partidos pendientes
    function generateTable() {
      tableBody.innerHTML = ""; // Limpiar tabla
      const start = (currentPage - 1) * itemsPerPage;
      const end = start + itemsPerPage;
      const pageData = pendingMatchesData.slice(start, end);
      pageData.forEach(match => {
        const newRow = `
          <tr>
            <td>${match.partido}</td>
            <td>${match.fecha}</td>
            <td>${match.hora}</td>
            <td>${match.locacion}</td>
          </tr>
        `;
        tableBody.innerHTML += newRow;
      });
      generatePagination(); // Generar la paginación después de actualizar la tabla
    }

    // Función para generar la paginación de partidos pendientes
    function generatePagination() {
      paginationMenu.innerHTML = ""; // Limpiar paginación
      const totalPages = Math.ceil(pendingMatchesData.length / itemsPerPage);
      for (let i = 1; i <= totalPages; i++) {
        const pageItem = document.createElement("span");
        pageItem.textContent = i;
        pageItem.classList.add("pagination-item");
        if (i === currentPage) {
          pageItem.classList.add("active");
        }
        pageItem.addEventListener("click", function() {
          currentPage = i;
          generateTable(); // Actualizar la tabla al hacer clic en una página
          updateActivePage(); // Actualizar la página activa
        });
        paginationMenu.appendChild(pageItem);
      }
    }

    // Función para actualizar la página activa
    function updateActivePage() {
      const pageItems = paginationMenu.querySelectorAll(".pagination-item");
      pageItems.forEach(item => item.classList.remove("active"));
      pageItems[currentPage - 1].classList.add("active");
    }

    generateTable(); // Generar la tabla al cargar la página por primera vez
  }

  // Función para generar las filas de partidos finalizados
  function generateFinishedMatchesTable(currentPage) {
    const tableBody = document.querySelector("#finished-matches tbody");
    const paginationMenu = document.querySelector("#finished-matches + .pagination .pagination-menu");

    // Datos de los partidos finalizados
    const finishedMatchesData = [
      { partido: "Partido 3", fecha: "2024-04-20", hora: "14:00", locacion: "Estadio C" },
      { partido: "Partido 4", fecha: "2024-04-19", hora: "17:00", locacion: "Estadio D" },
      { partido: "Partido 4", fecha: "2024-04-19", hora: "17:00", locacion: "Estadio D" },
      { partido: "Partido 4", fecha: "2024-04-19", hora: "17:00", locacion: "Estadio D" },
      { partido: "Partido 4", fecha: "2024-04-19", hora: "17:00", locacion: "Estadio D" },
      { partido: "Partido 4", fecha: "2024-04-19", hora: "17:00", locacion: "Estadio D" },
      { partido: "Partido 4", fecha: "2024-04-19", hora: "17:00", locacion: "Estadio D" },
      { partido: "Partido 4", fecha: "2024-04-19", hora: "17:00", locacion: "Estadio D" },
      { partido: "Partido 4", fecha: "2024-04-19", hora: "17:00", locacion: "Estadio D" },
      { partido: "Partido 4", fecha: "2024-04-19", hora: "17:00", locacion: "Estadio D" },
      { partido: "Partido 4", fecha: "2024-04-19", hora: "17:00", locacion: "Estadio D" },
      // Agrega más datos de partidos finalizados aquí según sea necesario
    ];

    const itemsPerPage = 10;

    // Función para generar la tabla de partidos finalizados
    function generateTable() {
      tableBody.innerHTML = ""; // Limpiar tabla
      const start = (currentPage - 1) * itemsPerPage;
      const end = start + itemsPerPage;
      const pageData = finishedMatchesData.slice(start, end);
      pageData.forEach(match => {
        const newRow = `
          <tr>
            <td>${match.partido}</td>
            <td>${match.fecha}</td>
            <td>${match.hora}</td>
            <td>${match.locacion}</td>
          </tr>
        `;
        tableBody.innerHTML += newRow;
      });
      generatePagination(); // Generar la paginación después de actualizar la tabla
    }

    // Función para generar la paginación de partidos finalizados
    function generatePagination() {
      paginationMenu.innerHTML = ""; // Limpiar paginación
      const totalPages = Math.ceil(finishedMatchesData.length / itemsPerPage);
      for (let i = 1; i <= totalPages; i++) {
        const pageItem = document.createElement("span");
        pageItem.textContent = i;
        pageItem.classList.add("pagination-item");
        if (i === currentPage) {
          pageItem.classList.add("active");
        }
        pageItem.addEventListener("click", function() {
          currentPage = i;
          generateTable(); // Actualizar la tabla al hacer clic en una página
          updateActivePage(); // Actualizar la página activa
        });
        paginationMenu.appendChild(pageItem);
      }
    }

    // Función para actualizar la página activa
    function updateActivePage() {
      const pageItems = paginationMenu.querySelectorAll(".pagination-item");
      pageItems.forEach(item => item.classList.remove("active"));
      pageItems[currentPage - 1].classList.add("active");
    }

    generateTable(); // Generar la tabla al cargar la página por primera vez
  }

  generatePendingMatchesTable(1); // Generar la tabla de partidos pendientes
  generateFinishedMatchesTable(1); // Generar la tabla de partidos finalizados
});
