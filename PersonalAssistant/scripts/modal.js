function attachModal(modalContainerId, modalContentId, launchButtonId, closeSpanId) {

    var modalContainer = document.getElementById(modalContainerId);
    var launchButton = document.getElementById(launchButtonId);
    var closeSpan = document.getElementById(closeSpanId);

    launchButton.onclick = function () {
        modalContainer.style.display = "block";
    }

    closeSpan.onclick = function () {
        modalContainer.style.display = "none";
    }

    window.onclick = function (event) {
        if (event.target == modalContainer) {
            modalContainer.style.display = "none";
        }
    }
}
