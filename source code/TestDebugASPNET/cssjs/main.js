// Simple message box

function showGoodMessage(title, message) {
    showMessage(title, message, true);
}

function showErrorMessage(title, message) {
    showMessage(title, message, false);
}

const activeMessages = [];

function showMessage(title, message, isSuccess) {
    // Create message container
    const container = document.createElement('div');
    container.className = 'sp-message-container';
    container.classList.add(isSuccess ? 'sp-message-success' : 'sp-message-error');

    // Create unique ID for this message
    const messageId = Date.now() + Math.random();
    container.dataset.messageId = messageId;

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

    // Add to active messages array
    activeMessages.push({
        id: messageId,
        element: container,
        timeout: null
    });

    // Position the message
    updateMessagePositions();

    // Show animation
    setTimeout(() => {
        container.classList.add('sp-message-show');
    }, 10);

    // Store reference to the message object
    const messageObj = activeMessages.find(m => m.id === messageId);

    // Auto-remove after 2500ms
    messageObj.timeout = setTimeout(() => {
        removeMessage(messageId);
    }, 2700);

    // Add click event listener to manually remove
    container.addEventListener('click', () => {
        if (messageObj.timeout) {
            clearTimeout(messageObj.timeout);
        }
        removeMessage(messageId);
    });
}

function removeMessage(messageId) {
    const index = activeMessages.findIndex(m => m.id === messageId);
    if (index === -1) return;

    const messageObj = activeMessages[index];
    const container = messageObj.element;

    // Hide animation
    container.classList.remove('sp-message-show');

    // Remove from array
    activeMessages.splice(index, 1);

    // Remove from DOM after animation
    setTimeout(() => {
        container.remove();
        // Update positions of remaining messages
        updateMessagePositions();
    }, 300);
}

function updateMessagePositions() {
    let currentTop = 30;

    activeMessages.forEach((messageObj, index) => {
        const container = messageObj.element;
        container.style.top = currentTop + 'px';

        // Calculate position for next message
        const containerHeight = container.offsetHeight;
        currentTop += containerHeight + 20; // 20px margin between messages
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

// Big Loading Animation

function showBigLoading(secondsTimeOut) {

    if (secondsTimeOut == null) {
        secondsTimeOut = 3000;
    }

    // Close any existing loading first
    closeBigLoading();

    // Create the overlay div
    const overlay = document.createElement('div');
    overlay.id = 'bigLoadingOverlay';

    // Create the spinner
    const spinner = document.createElement('div');
    spinner.id = 'loadingSpinner';

    // Append spinner to overlay
    overlay.appendChild(spinner);

    // Add to document body
    document.body.appendChild(overlay);

    // Add click event listener to close when clicked
    overlay.addEventListener('click', closeBigLoading);

    if (secondsTimeOut > 0) {
        // Store timeout ID on the overlay element itself
        overlay.timeoutId = setTimeout(() => {
            closeBigLoading();
        }, secondsTimeOut);
    }
}

function closeBigLoading() {
    const overlay = document.getElementById('bigLoadingOverlay');
    if (overlay) {
        // Clear the timeout stored on this specific overlay
        if (overlay.timeoutId) {
            clearTimeout(overlay.timeoutId);
        }
        overlay.remove();
    }
}