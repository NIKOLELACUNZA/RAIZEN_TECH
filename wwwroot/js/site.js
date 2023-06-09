﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
  var swiper = new Swiper(".swiper-container", {
    effect: "coverflow",
    grabCursor: true,
    centeredSlides: true,
    slidesPerView: "auto",
    coverflowEffect: {
      rotate: 50,
      stretch: 0,
      depth: 100,
      modifier: 1,
      slideShadows: true,
    },
    pagination: {
      el: ".swiper-pagination",
    },
  });
  

});

function showFullDescription(descriptionId, link) {
    var descriptionElement = document.getElementById(
      "description-" + descriptionId
    );
    if (descriptionElement.style.display === "none") {
      descriptionElement.style.display = "inline";
      link.innerHTML = "Ver menos";
    } else {
      descriptionElement.style.display = "none";
      link.innerHTML = "Ver más";
    }
}