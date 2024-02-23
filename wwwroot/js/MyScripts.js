//подтверждение действия
function ShowWindowConfirm(text) {
    return confirm(text);
};

$(document).ready(function () {

    $.validator.methods.range = function (value, element, param) {
        var globalizedValue = value.replace(",", ".");
        return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
    };

    $.validator.methods.number = function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
    };

    // модальное окно на подтверждение удаления
    let modalDelete = document.getElementById('ModalDelete')
    modalDelete.addEventListener('show.bs.modal', function (event) {
        let buttonDelete = event.relatedTarget
        let IDRecord = buttonDelete.getAttribute('data-bs-ID')
        let NameSector = buttonDelete.getAttribute('data-bs-Sector')
        let modalBody = modalDelete.querySelector('.modal-body')
        let modalFooterInput = modalDelete.querySelector('.modal-footer input')
        modalBody.textContent = `Вы точно хочете удалить участок: ${NameSector}`
        let ValueModalFooterInput = modalFooterInput.getAttribute('formaction')
        if (ValueModalFooterInput.includes('IsPlan')) {
            ValueModalFooterInput = ValueModalFooterInput.split('?')[0];
            modalFooterInput.setAttribute('formaction', `${ValueModalFooterInput}?IDRecord=${Number(IDRecord)}&IsPlan=true`);
        }
        else {
            if (ValueModalFooterInput.includes('?IDRecord')) { ValueModalFooterInput = ValueModalFooterInput.split('?')[0]; }

            modalFooterInput.setAttribute('formaction', `${ValueModalFooterInput}?IDRecord=${Number(IDRecord)}`);
        }
    });


   
});
    // таймер на информационную всплывающую подсказку
setTimeout(function () {
    document.getElementById('InfoOutTime').remove();
}, 2000);

    //кнопка "Наверх" при скролле
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 60 || document.documentElement.scrollTop > 60) {
        document.getElementById("topBtn").style.display = "block";
    } else {
        document.getElementById("topBtn").style.display = "none";
    }
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
}