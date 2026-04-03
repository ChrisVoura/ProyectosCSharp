// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function setNavbarTheme(theme) {
  const nav = document.querySelector(".navbar");
  if (!nav) return;

  if (theme === "dark") {
    nav.classList.remove("navbar-light", "bg-white");
    nav.classList.add("navbar-dark", "bg-dark");
  } else {
    nav.classList.remove("navbar-dark", "bg-dark");
    nav.classList.add("navbar-light", "bg-white");
  }
}

function setThemeIcon(theme) {
  const themeIcon = document.getElementById("themeIcon");
  if (!themeIcon) return;

  if (theme === "dark") {
    themeIcon.classList.remove("bi-moon-stars-fill");
    themeIcon.classList.add("bi-sun-fill", "text-warning");
  } else {
    themeIcon.classList.remove("bi-sun-fill", "text-warning");
    themeIcon.classList.add("bi-moon-stars-fill", "text-secondary");
  }
}

function applyTheme(theme) {
  const htmlElement = document.documentElement;
  htmlElement.setAttribute("data-bs-theme", theme);
  localStorage.setItem("theme", theme);

  const themeSwitch = document.getElementById("themeSwitch");
  if (themeSwitch) {
    themeSwitch.checked = theme === "dark";
  }

  setNavbarTheme(theme);
  setThemeIcon(theme);
  htmlElement.classList.add("theme-transition");
}

function initThemeSwitcher() {
  const savedTheme = localStorage.getItem("theme") || "light";
  applyTheme(savedTheme);

  const themeSwitch = document.getElementById("themeSwitch");
  if (!themeSwitch) return;

  themeSwitch.addEventListener("change", () => {
    const newTheme = themeSwitch.checked ? "dark" : "light";
    applyTheme(newTheme);
  });
}

if (document.readyState === "loading") {
  document.addEventListener("DOMContentLoaded", initThemeSwitcher);
} else {
  initThemeSwitcher();
}

async function actualizarIconoCarrito() {
  try {
    const res = await fetch("/Carrito?handler=CarritoCount");
    const data = await res.json();
    const img = document.getElementById("carritoIcon");
    if (img) {
      img.src =
        data.count > 0
          ? "https://img.icons8.com/keek-line/48/shopping-cart-loaded.png"
          : "https://img.icons8.com/keek-line/48/shopping-cart.png";
    }
  } catch (e) {}
}
document.addEventListener("DOMContentLoaded", actualizarIconoCarrito);

async function cargarCarrito() {
  const res = await fetch("/Carrito?handler=Contenido");
  document.getElementById("carritoContenido").innerHTML = await res.text();
}

async function actualizarIconoCarrito() {
  try {
    const res = await fetch("/Carrito?handler=CarritoCount");
    const data = await res.json();
    const img = document.getElementById("carritoIcon");
    if (img) {
      img.src =
        data.count > 0
          ? "https://img.icons8.com/keek-line/48/shopping-cart-loaded.png"
          : "https://img.icons8.com/keek-line/48/shopping-cart.png";
    }
  } catch (e) {}
}
async function limpiarCarrito() {
  await fetch("/Carrito?handler=Limpiar");
  await cargarCarrito();
  await actualizarIconoCarrito();
}

document.addEventListener("DOMContentLoaded", function () {
  actualizarIconoCarrito();
  document
    .getElementById("carritoOffcanvas")
    .addEventListener("show.bs.offcanvas", cargarCarrito);
});



