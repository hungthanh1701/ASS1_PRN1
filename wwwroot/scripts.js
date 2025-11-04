function navigate(section) {
       const content = document.getElementById('content');
       content.innerHTML = `<h2>${section} Section</h2><p>Manage ${section} here.</p>`;
   }