"use strict"

function SuccessMessage(title, messageBody, footer = '') {
    Swal.fire({
        icon: 'success',
        title: title,
        text: messageBody,
        footer: footer,
        allowOutsideClick: false
    });
}

function ErrorMessage(title, messageBody, footer = '') {
    Swal.fire({
        icon: 'error',
        title: title,
        text: messageBody,
        footer: footer,
        allowOutsideClick: false
    });
}

function WarningMessage(title, messageBody, footer = '') {
    Swal.fire({
        icon: 'warning',
        title: title,
        text: messageBody,
        footer: footer,
        allowOutsideClick: false
    });
}

function InformationMessage(title, messageBody, footer = '') {
    Swal.fire({
        icon: 'info',
        title: title,
        text: messageBody,
        footer: footer,
        allowOutsideClick: false
    });
}

function ConfirmationMessage(title, messageBody, icon, confirmBtnText, successTitle, successMessageBody, funcName, funcParam) {
    Swal.fire({
        title: title,
        text: messageBody,
        icon: icon,
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: confirmBtnText,
        allowOutsideClick: false,
    }).then((result) => {
        if (result.isConfirmed) {
            if (funcName != '' && funcName != null) {
                if (funcParam != '' && funcParam != null) {
                    window[funcName](funcParam);
                }
                else {
                    window[funcName]();
                }
            }
            else {
                SuccessMessage(successTitle, successMessageBody);
            }
        }
    });
}

function ConfirmationMessageForDeleteData(title, messageBody, successTitle, successMessageBody, dotnetHelper, methodAction) {
    const getLoader = document.getElementById("loaderFacility");
    const display = window.getComputedStyle(getLoader).display;
    if (display === "none") {
        getLoader.style.display = "flex";
    }

    Swal.fire({
        title: title,
        text: messageBody,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!',
        allowOutsideClick: false,
    }).then((result) => {
        if (result.isConfirmed) {
            if (methodAction !== '' || methodAction !== null) {
                dotnetHelper.invokeMethodAsync(methodAction);
            }
            else {
                Swal.fire(
                    successTitle,
                    successMessageBody,
                    'success'
                )
            }
        }
        else {
            getLoader.style.display = "none";
        }
    });
}

function OnActiveCurrentPage(element) {
    let getParentMenu = document.getElementById('menu-nav-user');
    let getMenuListElement = document.getElementsByClassName('nav-item');
    let menuKeyItem = element.dataset.usermenuId;
    let menuKey = 1;

    for (var i = 0; i < getMenuListElement.length; i++) {
        try {
            var getItemParentMenu = getMenuListElement[i];
            var getChildClassList = getItemParentMenu.getElementsByClassName('nav-link');

            for (var bb = 0; bb < getChildClassList.length; bb++) {
                try {
                    var getMenuInDropdown = document.getElementsByClassName('dropdown-item');
                    for (var kk = 0; kk < getMenuInDropdown.length; kk++) {
                        var getMenuDropdown = getMenuInDropdown[kk];
                        var getMenuDropdownClassList = getMenuDropdown.classList;
                        if (getMenuDropdownClassList.contains("active") || getMenuDropdownClassList.contains("on-menu-if-active")) {
                            getMenuDropdownClassList.remove("active");
                            getMenuDropdownClassList.remove("on-menu-if-active");
                        }
                    }

                    var child = getChildClassList[bb];
                    var childClassList = child.classList;
                    if (childClassList.contains("active") || childClassList.contains("on-menu-if-active")) {
                        childClassList.remove("active");
                        childClassList.remove("on-menu-if-active");
                    }
                } catch (e) {
                    console.error(e);
                }
            }
        } catch (e) {
            console.error(e);
        }
    }

    if (element.id === menuKeyItem) {
        element.classList.add("active");
        element.classList.add("on-menu-if-active");
    }
}
