<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="ReportProgressRestoreFileUpload.aspx.cs" Inherits="System.pages.ReportProgressRestoreFileUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .upload-container {
            border: 2px dashed #ccc;
            border-radius: 10px;
            padding: 40px;
            text-align: center;
            margin-bottom: 20px;
        }

        .file-input {
            margin: 20px 0;
        }

        .upload-btn {
            background-color: #007bff;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
        }

            .upload-btn:hover {
                background-color: #0056b3;
            }

            .upload-btn:disabled {
                background-color: #ccc;
                cursor: not-allowed;
            }

        .status {
            margin-top: 20px;
            padding: 10px;
            border-radius: 5px;
        }

            .status.success {
                background-color: #d4edda;
                color: #155724;
                border: 1px solid #c3e6cb;
            }

            .status.error {
                background-color: #f8d7da;
                color: #721c24;
                border: 1px solid #f5c6cb;
            }

            .status.loading {
                background-color: #d1ecf1;
                color: #0c5460;
                border: 1px solid #bee5eb;
            }

        .progress-bar {
            width: 100%;
            height: 20px;
            background-color: #f0f0f0;
            border-radius: 10px;
            overflow: hidden;
            margin: 10px 0;
        }

        .progress-fill {
            height: 100%;
            background-color: #007bff;
            width: 0%;
            transition: width 0.3s ease;
        }

        .progress-text {
            text-align: center;
            margin: 5px 0;
            font-size: 14px;
            color: #666;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">

        <div style="max-width: 800px;">
            <h1>Restore/Import MySQL Database - Progress Report</h1>

            <div class="upload-container">
                <h3>Select a SQL dump file or ZIP (contains SQL dump) to restore/import</h3>
                <input type="file" id="fileInput" class="file-input">
                <br>
                <button id="uploadBtn" type="button" class="upload-btn">Upload File</button>
            </div>

            <div class="progress-bar" id="progressBar" style="display: none;">
                <div class="progress-fill" id="progressFill"></div>
            </div>
            <div class="progress-text" id="progressText"></div>

            <div id="status"></div>

        </div>
    </div>

    <script>
        const fileInput = document.getElementById('fileInput');
        const uploadBtn = document.getElementById('uploadBtn');
        const statusDiv = document.getElementById('status');
        const progressBar = document.getElementById('progressBar');
        const progressFill = document.getElementById('progressFill');
        const progressText = document.getElementById('progressText');



        // Upload file with progress tracking using XMLHttpRequest
        function uploadWithProgress(file) {
            return new Promise((resolve, reject) => {

                const formData = new FormData();
                formData.append('file', file);

                // You can append additional data
                //formData.append('userId', '123');
                //formData.append('category', 'documents');

                const xhr = new XMLHttpRequest();

                // Track upload progress
                xhr.upload.addEventListener('progress', (e) => {
                    if (e.lengthComputable) {
                        const percentComplete = Math.round((e.loaded / e.total) * 100);
                        progressFill.style.width = percentComplete + '%';
                        progressText.textContent = `${percentComplete}% (${formatBytes(e.loaded)} / ${formatBytes(e.total)})`;
                    }
                });

                // Handle successful response
                xhr.addEventListener('load', () => {
                    if (xhr.status >= 200 && xhr.status < 300) {
                        const responseText = xhr.responseText.trim();

                        // Check if response starts with "1" (success) or "0" (error)
                        if (responseText === "1") {
                            resolve({ success: true, message: 'Upload successful' });
                        } else if (responseText.startsWith("0|")) {
                            // Extract error message after "0|"
                            const errorMessage = responseText.substring(2);
                            reject(new Error(errorMessage || 'Upload failed'));
                        } else {
                            // Unexpected response format
                            reject(new Error('Unexpected response format'));
                        }
                    } else {
                        reject(new Error(`HTTP error! status: ${xhr.status}`));
                    }
                });

                // Handle network errors
                xhr.addEventListener('error', (e) => {
                    console.error('XHR Error event:', e);
                    reject(new Error('Network error occurred'));
                });

                // Handle request timeout
                xhr.addEventListener('timeout', (e) => {
                    console.error('XHR Timeout event:', e);
                    reject(new Error('Request timed out'));
                });

                // Add readystatechange for debugging
                xhr.addEventListener('readystatechange', () => {
                    console.log('XHR State:', xhr.readyState, 'Status:', xhr.status);
                });

                // Handle upload start
                xhr.upload.addEventListener('loadstart', () => {
                    progressText.textContent = 'Starting upload...';
                });

                // Handle upload completion
                xhr.upload.addEventListener('load', () => {
                    progressText.textContent = 'Upload complete, processing...';
                });

                // Configure and send request
                xhr.open('POST', '/apiProgressReport?action=restore');

                // Set headers if needed
                // xhr.setRequestHeader('Authorization', 'Bearer your-token-here');

                xhr.send(formData);
            });
        }

        // Main upload handler - synchronous until confirmation
        uploadBtn.addEventListener('click', () => {

            beginProcessAsync();

        });

        // This is the only async function
        async function beginProcessAsync() {
            const file = fileInput.files[0];

            if (!file) {
                showStatus('Please select a file first', 'error');
                return;
            }

            uploadBtn.disabled = true;
            progressBar.style.display = 'block';

            try {
                showStatus('Uploading file...', 'loading');
                const result = await uploadWithProgress(file); // Only async operation
                showStatus(`Upload successful! File: ${file.name}`, 'success');
                window.location.href = "/ReportProgress";
                //fileInput.value = '';
                //spShowConfirmDialog(
                //    "File Upload Success",
                //    "Do you want to view the restore progress?",
                //    () => {
                //        // yes
                //        window.location.href = "/ReportProgress";
                //    },
                //    () => {
                //        // No, do nothing
                //    },);
                //console.log('Upload result:', result);
            } catch (error) {
                showStatus(`Upload failed: ${error.message}`, 'error');
                console.error('Upload error:', error);
            } finally {
                uploadBtn.disabled = false;
                setTimeout(() => {
                    progressBar.style.display = 'none';
                    progressFill.style.width = '0%';
                    progressText.textContent = '';
                }, 2000);
            }
        }

        function showStatus(message, type) {
            statusDiv.textContent = message;
            statusDiv.className = `status ${type}`;
        }

        function formatBytes(bytes) {
            if (bytes === 0) return '0 Bytes';
            const k = 1024;
            const sizes = ['Bytes', 'KB', 'MB', 'GB'];
            const i = Math.floor(Math.log(bytes) / Math.log(k));
            return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
        }

        // Drag and drop functionality - modified for single file
        const uploadContainer = document.querySelector('.upload-container');

        uploadContainer.addEventListener('dragover', (e) => {
            e.preventDefault();
            uploadContainer.style.borderColor = '#007bff';
            uploadContainer.style.backgroundColor = '#f8f9fa';
        });

        uploadContainer.addEventListener('dragleave', (e) => {
            e.preventDefault();
            uploadContainer.style.borderColor = '#ccc';
            uploadContainer.style.backgroundColor = 'transparent';
        });

        uploadContainer.addEventListener('drop', (e) => {
            e.preventDefault();
            uploadContainer.style.borderColor = '#ccc';
            uploadContainer.style.backgroundColor = 'transparent';

            const files = e.dataTransfer.files;

            if (files.length > 1) {
                showStatus('Please drop only one file at a time', 'error');
                return;
            }

            if (files.length === 1) {
                // Create a new FileList with just the first file
                const dt = new DataTransfer();
                dt.items.add(files[0]);
                fileInput.files = dt.files;

                showStatus(`File selected via drag & drop: ${files[0].name}`, 'success');
            }
        });
    </script>

</asp:Content>
