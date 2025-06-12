// Simple message box

function showGoodMessage(title, message) {
    showMessage(title, message, true);
}

function showErrorMessage(title, message) {
    showMessage(title, message, false);
}

function showMessage(title, message, isSuccess) {
    // Remove existing message if any
    const existingMessage = document.querySelector('.sp-message-container');
    if (existingMessage) {
        existingMessage.remove();
    }

    // Create message container
    const container = document.createElement('div');
    container.className = 'sp-message-container';
    container.classList.add(isSuccess ? 'sp-message-success' : 'sp-message-error');

    // Create title
    const titleEl = document.createElement('div');
    titleEl.className = 'sp-message-title';
    titleEl.textContent = title;

    // Create message
    const messageEl = document.createElement('div');
    messageEl.className = 'sp-message-text';
    messageEl.textContent = message;

    // Append elements
    container.appendChild(titleEl);
    container.appendChild(messageEl);
    document.body.appendChild(container);

    // Slide down animation
    setTimeout(() => {
        container.classList.add('sp-message-show');
    }, 10);

    // Slide up and remove after 1800ms (unless clicked)
    const autoRemoveTimeout = setTimeout(() => {
        container.classList.remove('sp-message-show');
        setTimeout(() => {
            container.remove();
        }, 300); // Wait for slide up animation to complete
    }, 1800);

    // Add click event listener to manually remove
    container.addEventListener('click', () => {
        clearTimeout(autoRemoveTimeout); // Prevent automatic removal
        container.classList.remove('sp-message-show');
        setTimeout(() => {
            container.remove();
        }, 300); // Wait for slide up animation to complete
    });
}

// Dialog Action Box

function spShowConfirmDialog(title, message, customData, onYesCallback, onNoCallback) {
    // Same function implementation as before
    const existingDialog = document.querySelector('.sp-dialog-container');
    if (existingDialog) {
        existingDialog.remove();
    }

    const container = document.createElement('div');
    container.className = 'sp-dialog-container';
    container.setAttribute('data-field', customData);

    const titleEl = document.createElement('div');
    titleEl.className = 'sp-dialog-title';
    titleEl.textContent = title;

    const messageEl = document.createElement('div');
    messageEl.className = 'sp-dialog-text';
    messageEl.textContent = message;

    const buttonsContainer = document.createElement('div');
    buttonsContainer.className = 'sp-dialog-buttons';

    const yesButton = document.createElement('button');
    yesButton.className = 'sp-dialog-button sp-dialog-yes';
    yesButton.textContent = 'Yes';

    const noButton = document.createElement('button');
    noButton.className = 'sp-dialog-button sp-dialog-no';
    noButton.textContent = 'No';

    buttonsContainer.appendChild(yesButton);
    buttonsContainer.appendChild(noButton);
    container.appendChild(titleEl);
    container.appendChild(messageEl);
    container.appendChild(buttonsContainer);
    document.body.appendChild(container);

    setTimeout(() => {
        container.classList.add('sp-dialog-show');
    }, 10);

    yesButton.addEventListener('click', () => {
        const data = container.getAttribute('data-field');
        container.classList.remove('sp-dialog-show');
        setTimeout(() => {
            container.remove();
            if (onYesCallback) onYesCallback(data);
        }, 300);
    });

    noButton.addEventListener('click', () => {
        const data = container.getAttribute('data-field');
        container.classList.remove('sp-dialog-show');
        setTimeout(() => {
            container.remove();
            if (onNoCallback) onNoCallback(data);
        }, 300);
    });
}