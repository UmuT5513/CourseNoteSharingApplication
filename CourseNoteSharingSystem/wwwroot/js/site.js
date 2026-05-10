(() => {
    const themeButton = document.querySelector("[data-theme-toggle]");
    const root = document.documentElement;

    function setTheme(theme) {
        root.setAttribute("data-theme", theme);
        localStorage.setItem("courseNoteTheme", theme);
        if (themeButton) {
            themeButton.innerHTML = theme === "dark" ? '<i class="bi bi-sun"></i>' : '<i class="bi bi-moon-stars"></i>';
        }
    }

    setTheme(localStorage.getItem("courseNoteTheme") || "light");
    themeButton?.addEventListener("click", () => {
        setTheme(root.getAttribute("data-theme") === "dark" ? "light" : "dark");
    });

    document.querySelectorAll("[data-sidebar-toggle]").forEach((toggle) => {
        toggle.addEventListener("click", () => {
            document.querySelector("[data-sidebar]")?.classList.toggle("is-open");
            document.querySelector(".sidebar-backdrop")?.classList.toggle("is-open");
        });
    });

    document.querySelectorAll(".toast").forEach((toast) => new bootstrap.Toast(toast).show());

    document.querySelectorAll("[data-password-toggle]").forEach((button) => {
        button.addEventListener("click", () => {
            const input = button.closest(".floating-field, .input-group, .position-relative")?.querySelector("input");
            if (!input) return;
            input.type = input.type === "password" ? "text" : "password";
            button.innerHTML = input.type === "password" ? '<i class="bi bi-eye"></i>' : '<i class="bi bi-eye-slash"></i>';
        });
    });

    const uploadInput = document.querySelector("[data-upload-input]");
    const uploadZone = document.querySelector("[data-upload-zone]");
    const fileName = document.querySelector("[data-file-name]");
    const progress = document.querySelector("[data-upload-progress]");

    if (uploadInput && uploadZone) {
        ["dragenter", "dragover"].forEach((eventName) => {
            uploadZone.addEventListener(eventName, (event) => {
                event.preventDefault();
                uploadZone.classList.add("is-dragover");
            });
        });
        ["dragleave", "drop"].forEach((eventName) => {
            uploadZone.addEventListener(eventName, (event) => {
                event.preventDefault();
                uploadZone.classList.remove("is-dragover");
            });
        });
        uploadZone.addEventListener("drop", (event) => {
            uploadInput.files = event.dataTransfer.files;
            uploadInput.dispatchEvent(new Event("change"));
        });
        uploadInput.addEventListener("change", () => {
            const file = uploadInput.files?.[0];
            if (!file) return;
            if (fileName) fileName.textContent = `${file.name} · ${(file.size / 1024 / 1024).toFixed(2)} MB`;
            if (progress) {
                progress.style.width = "0%";
                requestAnimationFrame(() => progress.style.width = "100%");
            }
        });
    }

    document.querySelectorAll("[data-filter-input]").forEach((input) => {
        input.addEventListener("input", () => {
            const query = input.value.toLowerCase();
            document.querySelectorAll("[data-filter-item]").forEach((item) => {
                item.classList.toggle("d-none", !item.textContent.toLowerCase().includes(query));
            });
        });
    });
})();
