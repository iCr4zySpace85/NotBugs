// Objeto que mapea el ID del enlace con el ID del contenido
var contenidoPorEnlace = {
    usuarioL: 'usuarioContenido',
    rolesL: 'rolesContenido',
    permisosL: 'permisosContenido',  
    inscripL: 'usuarioInscripciones',
    arbitrajeL: 'usuarioArbitraje',
    patrocinadoresL: 'usuarioPatro',
    gastosL: 'usuarioGastos',
  };
  
  // Función para mostrar el contenido correspondiente al hacer clic en un enlace
  function mostrarContenido(idContenido) {
    // Oculta todos los contenidos
    var contenidos = document.querySelectorAll('.contenido');
    contenidos.forEach(function(contenido) {
      contenido.style.display = 'none';
    });
  
    // Muestra el contenido correspondiente al enlace clickeado
    var contenidoMostrado = document.getElementById(idContenido);
    contenidoMostrado.style.display = 'block';
  }
  
  // Agrega event listener a cada enlace del menú para mostrar el contenido correspondiente
  var enlacesMenu = document.querySelectorAll('#menuItems a');
  enlacesMenu.forEach(function(enlace) {
    enlace.addEventListener('click', function(event) {
      event.preventDefault(); // Evita que el enlace recargue la página
      var idEnlace = this.id;
      var idContenido = contenidoPorEnlace[idEnlace];
      mostrarContenido(idContenido);
    });
  });
  