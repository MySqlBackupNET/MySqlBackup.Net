
function loadThemeLight() {
    styleTheme.innerText = `
        input[type="file"] {
            margin: 0 0 20px 10px;
            padding: 8px;
            border: 2px dashed #ddd;
            border-radius: 6px;
            background: white;
            font-size: 14px;
            cursor: pointer;
            transition: border-color 0.3s ease;
        }

            input[type="file"]:hover {
                border-color: #4CAF50;
            }

        #progress_bar_container {
            background: linear-gradient(90deg, #2a2a2a, #1a1a1a, #2a2a2a);
            background-size: 200% 100%;
            animation: backgroundShift 3s ease-in-out infinite;
            border-radius: 25px;
            margin: 0 0 10px 0;
            box-shadow: inset 0 2px 8px rgba(0,0,0,0.4);
            height: 30px;
            position: relative;
            overflow: hidden;
            border: 1px solid #333;
            padding: 1px;
        }

        #progress_bar_indicator {
            background: linear-gradient(90deg, #00ff88, #00cc6a, #00ff88);
            background-size: 200% 100%;
            height: 100%;
            border-radius: 20px;
            width: 0%;
            transition: width 0.3s ease;
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
            animation: ambientBreathing 2s ease-in-out infinite, gradientFlow 3s linear infinite;
            box-shadow: 0 0 20px rgba(0, 255, 136, 0.6), 0 0 40px rgba(0, 255, 136, 0.3), inset 0 1px 0 rgba(255, 255, 255, 0.3);
        }

        @keyframes ambientBreathing {
            0%, 100% {
                box-shadow: 0 0 20px rgba(0, 255, 136, 0.6), 0 0 40px rgba(0, 255, 136, 0.3), inset 0 1px 0 rgba(255, 255, 255, 0.3);
                filter: brightness(1);
            }

            50% {
                box-shadow: 0 0 30px rgba(0, 255, 136, 0.8), 0 0 60px rgba(0, 255, 136, 0.5), 0 0 80px rgba(0, 255, 136, 0.2), inset 0 1px 0 rgba(255, 255, 255, 0.4);
                filter: brightness(1.2);
            }
        }

        @keyframes backgroundShift {
            0%, 100% {
                background-position: 0% 50%;
            }

            50% {
                background-position: 100% 50%;
            }
        }

        @keyframes gradientFlow {
            0% {
                background-position: 0% 50%;
            }

            100% {
                background-position: 200% 50%;
            }
        }

        #labelPercent {
            color: white;
            font-weight: bold;
            font-size: 14px;
            text-shadow: 0 0 10px rgba(0, 255, 136, 0.8), 1px 1px 2px rgba(0,0,0,0.8);
            z-index: 10;
        }

        .div_task_status table {
            border-collapse: collapse;
            font-size: 14px;
            line-height: 1.6;
        }

        .div_task_status td {
            padding: 12px 15px;
            border-bottom: 1px solid rgba(0,0,0,0.1);
            vertical-align: top;
        }

            .div_task_status td:first-child {
                font-weight: 600;
                color: #4CAF50;
                background: rgba(76, 175, 80, 0.05);
                border-right: 3px solid #4CAF50;
                width: 150px;
                text-align: right;
            }

            .div_task_status td:nth-child(2) {
                background: rgba(0,0,0,0.02);
                min-width: 150px;
            }

        .div_task_status tr:last-child td {
            border-bottom: none;
        }

        .status-complete {
            background: linear-gradient(135deg, rgba(76, 175, 80, 0.15), rgba(76, 175, 80, 0.05)) !important;
            border-left: 4px solid #4CAF50 !important;
            color: #2E7D32 !important;
            font-weight: bold;
            animation: statusGlow 2s ease-in-out infinite;
        }

        .status-running {
            background: linear-gradient(135deg, rgba(33, 150, 243, 0.15), rgba(33, 150, 243, 0.05)) !important;
            border-left: 4px solid #2196F3 !important;
            color: #1565C0 !important;
            font-weight: bold;
            animation: statusPulse 1.5s ease-in-out infinite;
        }

        .status-error {
            background: linear-gradient(135deg, rgba(244, 67, 54, 0.15), rgba(244, 67, 54, 0.05)) !important;
            border-left: 4px solid #F44336 !important;
            color: #C62828 !important;
            font-weight: bold;
            animation: statusShake 0.5s ease-in-out;
        }

        @keyframes statusGlow {
            0%, 100% {
                box-shadow: 0 0 5px rgba(76, 175, 80, 0.3);
            }

            50% {
                box-shadow: 0 0 15px rgba(76, 175, 80, 0.6), 0 0 25px rgba(76, 175, 80, 0.3);
            }
        }

        @keyframes statusPulse {
            0%, 100% {
                box-shadow: 0 0 5px rgba(33, 150, 243, 0.3);
                transform: scale(1);
            }

            50% {
                box-shadow: 0 0 15px rgba(33, 150, 243, 0.6), 0 0 25px rgba(33, 150, 243, 0.3);
                transform: scale(1.02);
            }
        }

        @keyframes statusShake {
            0%, 100% {
                transform: translateX(0);
            }

            25% {
                transform: translateX(-2px);
            }

            75% {
                transform: translateX(2px);
            }
        }
            `;
}

function loadThemeDark() {
    styleTheme.innerText = `
        /* Dark theme overrides */
        body {
            background: #1a1a1a;
        }
        
        .div-center-framed-content {
            background-color: #2a2a2a;
            box-shadow: 0 2px 8px rgba(0,0,0,0.5);
            border: 1px solid #3a3a3a;
        }
        
        .div-center-framed-content h1 {
            color: #e0e0e0;
        }
        
        input[type="file"] {
            margin: 0 0 20px 10px;
            padding: 8px;
            border: 2px dashed #555;
            border-radius: 6px;
            background: #333;
            color: #e0e0e0;
            font-size: 14px;
            cursor: pointer;
            transition: border-color 0.3s ease;
        }

        input[type="file"]:hover {
            border-color: #00ff88;
        }
        
        input[type=submit], button, .buttonmain, a.buttonmain {
            background: #1e4a7d;
            border: 1px solid #2a5a8d;
        }
        
        input[type=submit]:hover, button:hover, .buttonmain:hover, a.buttonmain:hover {
            background: #2a5a8d;
            box-shadow: 0 0 10px rgba(0, 150, 255, 0.3);
        }

        #progress_bar_container {
            background: linear-gradient(90deg, #0a0a0a, #151515, #0a0a0a);
            background-size: 200% 100%;
            animation: backgroundShift 3s ease-in-out infinite;
            border-radius: 25px;
            margin: 0 0 10px 0;
            box-shadow: inset 0 2px 12px rgba(0,0,0,0.8);
            height: 30px;
            position: relative;
            overflow: hidden;
            border: 1px solid #1a1a1a;
            padding: 1px;
        }

        #progress_bar_indicator {
            background: linear-gradient(90deg, #00ff88, #00cc6a, #00ff88, #00ffaa);
            background-size: 300% 100%;
            height: 100%;
            border-radius: 20px;
            width: 0%;
            transition: width 0.3s ease;
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
            animation: ambientBreathing 2s ease-in-out infinite, gradientFlow 4s linear infinite;
            box-shadow: 0 0 25px rgba(0, 255, 136, 0.8), 
                        0 0 50px rgba(0, 255, 136, 0.4), 
                        inset 0 1px 0 rgba(255, 255, 255, 0.4),
                        inset 0 -1px 0 rgba(0, 0, 0, 0.3);
        }

        @keyframes ambientBreathing {
            0%, 100% {
                box-shadow: 0 0 25px rgba(0, 255, 136, 0.8), 
                            0 0 50px rgba(0, 255, 136, 0.4), 
                            inset 0 1px 0 rgba(255, 255, 255, 0.4);
                filter: brightness(1);
            }

            50% {
                box-shadow: 0 0 35px rgba(0, 255, 136, 1), 
                            0 0 70px rgba(0, 255, 136, 0.6), 
                            0 0 100px rgba(0, 255, 136, 0.3), 
                            inset 0 1px 0 rgba(255, 255, 255, 0.5);
                filter: brightness(1.3);
            }
        }

        @keyframes backgroundShift {
            0%, 100% {
                background-position: 0% 50%;
            }

            50% {
                background-position: 100% 50%;
            }
        }

        @keyframes gradientFlow {
            0% {
                background-position: 0% 50%;
            }

            100% {
                background-position: 300% 50%;
            }
        }

        #labelPercent {
            color: #0a0a0a;
            font-weight: bold;
            font-size: 14px;
            text-shadow: 0 0 15px rgba(0, 255, 136, 1), 
                         0 0 5px rgba(255,255,255,0.8),
                         1px 1px 2px rgba(0,0,0,0.5);
            z-index: 10;
            mix-blend-mode: normal;
        }

        .div_task_status {
            color: #e0e0e0;
        }

        .div_task_status table {
            border-collapse: collapse;
            font-size: 14px;
            line-height: 1.6;
            color: #e0e0e0;
        }

        .div_task_status td {
            padding: 12px 15px;
            border-bottom: 1px solid rgba(255,255,255,0.1);
            vertical-align: top;
            color: #e0e0e0;
        }

        .div_task_status td:first-child {
            font-weight: 600;
            color: #00ff88;
            background: rgba(0, 255, 136, 0.1);
            border-right: 3px solid #00ff88;
            width: 150px;
            text-align: right;
            text-shadow: 0 0 5px rgba(0, 255, 136, 0.5);
        }

        .div_task_status td:nth-child(2) {
            background: rgba(255,255,255,0.02);
            min-width: 150px;
        }

        .div_task_status tr:last-child td {
            border-bottom: none;
        }

        .status-complete {
            background: linear-gradient(135deg, rgba(0, 255, 136, 0.2), rgba(0, 255, 136, 0.05)) !important;
            border-left: 4px solid #00ff88 !important;
            color: #00ff88 !important;
            font-weight: bold;
            animation: statusGlowGreen 2s ease-in-out infinite;
            text-shadow: 0 0 10px rgba(0, 255, 136, 0.8);
        }

        .status-running {
            background: linear-gradient(135deg, rgba(0, 150, 255, 0.2), rgba(0, 150, 255, 0.05)) !important;
            border-left: 4px solid #0096ff !important;
            color: #00b4ff !important;
            font-weight: bold;
            animation: statusPulseBlue 1.5s ease-in-out infinite;
            text-shadow: 0 0 10px rgba(0, 150, 255, 0.8);
        }

        .status-error {
            background: linear-gradient(135deg, rgba(255, 67, 54, 0.2), rgba(255, 67, 54, 0.05)) !important;
            border-left: 4px solid #ff4336 !important;
            color: #ff6b6b !important;
            font-weight: bold;
            animation: statusShake 0.5s ease-in-out;
            text-shadow: 0 0 10px rgba(255, 67, 54, 0.8);
        }

        @keyframes statusGlowGreen {
            0%, 100% {
                box-shadow: 0 0 10px rgba(0, 255, 136, 0.5);
            }

            50% {
                box-shadow: 0 0 25px rgba(0, 255, 136, 0.8), 
                            0 0 40px rgba(0, 255, 136, 0.4);
            }
        }

        @keyframes statusPulseBlue {
            0%, 100% {
                box-shadow: 0 0 10px rgba(0, 150, 255, 0.5);
                transform: scale(1);
            }

            50% {
                box-shadow: 0 0 25px rgba(0, 150, 255, 0.8), 
                            0 0 40px rgba(0, 150, 255, 0.4);
                transform: scale(1.02);
            }
        }

        @keyframes statusShake {
            0%, 100% {
                transform: translateX(0);
            }

            25% {
                transform: translateX(-2px);
            }

            75% {
                transform: translateX(2px);
            }
        }
        
        /* Links in dark theme */
        .div_task_status a, 
        .div_task_status a.mainbutton {
            color: #00b4ff;
            background: #1e4a7d;
            border: 1px solid #2a5a8d;
        }
        
        .div_task_status a:hover,
        .div_task_status a.mainbutton:hover {
            color: #00d4ff;
            background: #2a5a8d;
            text-decoration: none;
            box-shadow: 0 0 15px rgba(0, 180, 255, 0.5);
        }
        
        /* Additional dark theme specific styles */
        span {
            color: #e0e0e0;
        }
        
        /* Progress bar container subtle animation for dark theme */
        #progress_bar_container::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.05), transparent);
            animation: shimmer 3s infinite;
        }
        
        @keyframes shimmer {
            0% {
                left: -100%;
            }
            100% {
                left: 100%;
            }
        }
    `;
}

function loadThemeCyberpunk() {
    styleTheme.innerText = `
        /* Sci-Fi Dark Theme - Cyberpunk/Futuristic */
        body {
            background: #000;
            background-image: 
                radial-gradient(ellipse at top left, rgba(0, 255, 255, 0.1) 0%, transparent 40%),
                radial-gradient(ellipse at bottom right, rgba(255, 0, 255, 0.1) 0%, transparent 40%),
                linear-gradient(180deg, #000 0%, #0a0a0f 100%);
        }
        
        .div-center-framed-content {
            color: darkgray;
            background: rgba(5, 5, 15, 0.9);
            border: 1px solid #00ffff;
            box-shadow: 
                0 0 30px rgba(0, 255, 255, 0.3),
                inset 0 0 20px rgba(0, 255, 255, 0.1),
                0 0 60px rgba(255, 0, 255, 0.1);
            position: relative;
            overflow: hidden;
        }
        
        /* Animated corner accents */
        .div-center-framed-content::before,
        .div-center-framed-content::after {
            content: '';
            position: absolute;
            width: 20px;
            height: 20px;
            border: 2px solid #00ffff;
        }
        
        .div-center-framed-content::before {
            top: -1px;
            left: -1px;
            border-right: none;
            border-bottom: none;
            animation: cornerPulse 2s infinite;
        }
        
        .div-center-framed-content::after {
            bottom: -1px;
            right: -1px;
            border-left: none;
            border-top: none;
            animation: cornerPulse 2s infinite 1s;
        }
        
        @keyframes cornerPulse {
            0%, 100% { 
                box-shadow: 0 0 10px #00ffff;
                opacity: 1;
            }
            50% { 
                box-shadow: 0 0 20px #00ffff, 0 0 30px #00ffff;
                opacity: 0.7;
            }
        }
        
        .div-center-framed-content h1 {
            color: #00ffff;
            text-transform: uppercase;
            letter-spacing: 3px;
            text-shadow: 
                0 0 10px rgba(0, 255, 255, 0.8),
                0 0 20px rgba(0, 255, 255, 0.5),
                0 0 30px rgba(0, 255, 255, 0.3);
            font-weight: 300;
        }
        
        input[type="file"] {
            margin: 0 0 20px 10px;
            padding: 8px;
            border: 1px solid #00ffff;
            border-radius: 0;
            background: rgba(0, 255, 255, 0.05);
            color: #00ffff;
            font-size: 14px;
            cursor: pointer;
            transition: all 0.3s ease;
            position: relative;
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        input[type="file"]:hover {
            background: rgba(0, 255, 255, 0.1);
            box-shadow: 
                0 0 15px rgba(0, 255, 255, 0.5),
                inset 0 0 10px rgba(0, 255, 255, 0.2);
            text-shadow: 0 0 5px rgba(0, 255, 255, 0.8);
        }

        input[type="file"]::file-selector-button {
            padding: 8px;
            border: 1px solid #00ffff;
            border-radius: 0;
            background: rgba(0, 255, 255, 0.05);
            color: #00ffff;
            font-size: 14px;
            cursor: pointer;
        }

        input[type=submit], button, .buttonmain, a.buttonmain {
            background: transparent;
            border: 1px solid #ff00ff;
            color: #ff00ff;
            text-transform: uppercase;
            letter-spacing: 2px;
            position: relative;
            overflow: hidden;
            transition: all 0.3s;
        }
        
        input[type=submit]::before, button::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(255, 0, 255, 0.4), transparent);
            transition: left 0.5s;
        }
        
        input[type=submit]:hover::before, button:hover::before {
            left: 100%;
        }
        
        input[type=submit]:hover, button:hover, .buttonmain:hover, a.buttonmain:hover {
            color: #fff;
            background: rgba(255, 0, 255, 0.1);
            box-shadow: 
                0 0 20px rgba(255, 0, 255, 0.6),
                inset 0 0 10px rgba(255, 0, 255, 0.3);
            text-shadow: 0 0 10px #ff00ff;
        }

        #progress_bar_container {
            background: #000;
            border: 1px solid rgba(0, 255, 255, 0.3);
            border-radius: 0;
            margin: 0 0 10px 0;
            height: 30px;
            position: relative;
            overflow: hidden;
            box-shadow: 
                inset 0 0 20px rgba(0, 0, 0, 0.8),
                0 0 20px rgba(0, 255, 255, 0.2);
        }
        
        /* Sci-fi grid effect */
        #progress_bar_container::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-image: 
                repeating-linear-gradient(0deg, 
                    transparent, 
                    transparent 2px, 
                    rgba(0, 255, 255, 0.1) 2px, 
                    rgba(0, 255, 255, 0.1) 4px),
                repeating-linear-gradient(90deg, 
                    transparent, 
                    transparent 2px, 
                    rgba(0, 255, 255, 0.1) 2px, 
                    rgba(0, 255, 255, 0.1) 4px);
            animation: gridMove 10s linear infinite;
        }
        
        @keyframes gridMove {
            0% { transform: translate(0, 0); }
            100% { transform: translate(4px, 4px); }
        }

        #progress_bar_indicator {
            background: linear-gradient(90deg, 
                #ff00ff, 
                #00ffff, 
                #ff00ff, 
                #00ffff);
            background-size: 400% 100%;
            height: 100%;
            width: 0%;
            transition: width 0.3s ease;
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
            animation: cyberpunkGradient 3s linear infinite, hologramFlicker 0.1s infinite;
            box-shadow: 
                0 0 20px currentColor,
                0 0 40px currentColor,
                0 0 60px currentColor,
                inset 0 0 20px rgba(255, 255, 255, 0.5);
            filter: brightness(1.2) contrast(1.1);
        }

        @keyframes cyberpunkGradient {
            0% { background-position: 0% 50%; }
            100% { background-position: 400% 50%; }
        }
        
        @keyframes hologramFlicker {
            0%, 100% { opacity: 1; }
            92% { opacity: 1; }
            93% { opacity: 0.8; }
            94% { opacity: 1; }
        }

        #labelPercent {
            color: #000;
            background: rgba(255, 255, 255, 0.9);
            padding: 2px 8px;
            font-weight: bold;
            font-size: 12px;
            font-family: 'Courier New', monospace;
            text-transform: uppercase;
            letter-spacing: 2px;
            text-shadow: none;
            z-index: 10;
            border: 1px solid #00ffff;
            box-shadow: 0 0 10px rgba(0, 255, 255, 0.8);
        }

        .div_task_status {
            color: #00ffff;
            margin-top: 20px;
        }

        .div_task_status table {
            border-collapse: separate;
            border-spacing: 0;
            font-size: 14px;
            line-height: 1.6;
            color: #00ffff;
            border: 1px solid rgba(0, 255, 255, 0.3);
            background: rgba(0, 0, 0, 0.5);
            position: relative;
        }
        
        /* Holographic shimmer effect */
        .div_task_status table::before {
            content: '';
            position: absolute;
            top: -2px;
            left: -2px;
            right: -2px;
            bottom: -2px;
            background: linear-gradient(45deg, 
                #ff00ff, #00ffff, #ff00ff, #00ffff);
            z-index: -1;
            filter: blur(5px);
            opacity: 0.5;
            animation: holographicShimmer 3s linear infinite;
        }
        
        @keyframes holographicShimmer {
            0%, 100% { opacity: 0.5; }
            50% { opacity: 0.8; }
        }

        .div_task_status td {
            padding: 12px 15px;
            border-bottom: 1px solid rgba(0, 255, 255, 0.2);
            vertical-align: top;
            color: #0ff;
            background: rgba(0, 0, 0, 0.3);
        }

        .div_task_status td:first-child {
            font-weight: 600;
            color: #ff00ff;
            background: linear-gradient(90deg, 
                rgba(255, 0, 255, 0.1) 0%, 
                rgba(255, 0, 255, 0.05) 100%);
            border-right: 2px solid #ff00ff;
            width: 150px;
            text-align: right;
            text-transform: uppercase;
            letter-spacing: 1px;
            font-size: 12px;
            text-shadow: 0 0 10px rgba(255, 0, 255, 0.8);
        }

        .div_task_status td:nth-child(2) {
            background: rgba(0, 255, 255, 0.02);
            min-width: 150px;
            font-family: 'Courier New', monospace;
        }

        .div_task_status tr:last-child td {
            border-bottom: none;
        }

        .status-complete {
            background: linear-gradient(135deg, 
                rgba(0, 255, 0, 0.2), 
                rgba(0, 255, 0, 0.05)) !important;
            border-left: 4px solid #0f0 !important;
            color: #0f0 !important;
            font-weight: bold;
            text-transform: uppercase;
            letter-spacing: 2px;
            animation: dataStream 2s linear infinite;
            text-shadow: 0 0 10px #0f0;
        }

        .status-running {
            background: linear-gradient(135deg, 
                rgba(255, 255, 0, 0.2), 
                rgba(255, 255, 0, 0.05)) !important;
            border-left: 4px solid #ff0 !important;
            color: #ff0 !important;
            font-weight: bold;
            text-transform: uppercase;
            letter-spacing: 2px;
            animation: processingPulse 0.5s infinite alternate;
            text-shadow: 0 0 10px #ff0;
        }

        .status-error {
            background: linear-gradient(135deg, 
                rgba(255, 0, 0, 0.2), 
                rgba(255, 0, 0, 0.05)) !important;
            border-left: 4px solid #f00 !important;
            color: #f00 !important;
            font-weight: bold;
            text-transform: uppercase;
            letter-spacing: 2px;
            animation: alertFlash 0.3s infinite;
            text-shadow: 0 0 10px #f00;
        }

        @keyframes dataStream {
            0% { opacity: 1; }
            50% { opacity: 0.7; }
            100% { opacity: 1; }
        }

        @keyframes processingPulse {
            0% { transform: scale(1); filter: brightness(1); }
            100% { transform: scale(1.02); filter: brightness(1.3); }
        }

        @keyframes alertFlash {
            0%, 100% { opacity: 1; }
            50% { opacity: 0.6; }
        }
        
        /* Links in sci-fi theme */
        .div_task_status a, 
        .div_task_status a.mainbutton {
            color: #00ffff;
            background: transparent;
            border: 1px solid #00ffff;
            text-transform: uppercase;
            letter-spacing: 1px;
            position: relative;
            overflow: hidden;
        }
        
        .div_task_status a:hover,
        .div_task_status a.mainbutton:hover {
            color: #fff;
            background: rgba(0, 255, 255, 0.1);
            text-decoration: none;
            box-shadow: 
                0 0 20px rgba(0, 255, 255, 0.6),
                inset 0 0 10px rgba(0, 255, 255, 0.3);
            text-shadow: 0 0 10px #00ffff;
        }
        
        /* Additional sci-fi elements */
        span {
            color: #00ffff;
        }
        
        /* Scanline effect */
        .div-center-framed-content::after {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            height: 2px;
            background: linear-gradient(90deg, 
                transparent, 
                rgba(0, 255, 255, 0.8), 
                transparent);
            animation: scanline 8s linear infinite;
            pointer-events: none;
        }
        
        @keyframes scanline {
            0% { top: 0%; }
            100% { top: 100%; }
        }
        
        /* Data corruption glitch effect on hover */
        button:hover, input[type=submit]:hover {
            animation: glitch 0.3s infinite;
        }
        
        @keyframes glitch {
            0%, 100% { 
                text-shadow: 
                    0 0 10px #ff00ff,
                    2px 2px 0 #00ffff,
                    -2px -2px 0 #ff00ff;
            }
            50% { 
                text-shadow: 
                    0 0 10px #ff00ff,
                    -2px 2px 0 #00ffff,
                    2px -2px 0 #ff00ff;
            }
        }
    `;
}

function loadThemeTerminalAlien1986() {
    styleTheme.innerText = `
        /* Retro Terminal/Mainframe Theme - Phosphor CRT Style */
        @import url('https://fonts.googleapis.com/css2?family=VT323&display=swap');
        
        /* Global phosphor green theme */
        * {
            
        }
        
        body {
            background: #000;
            position: relative;
            overflow-x: hidden;
        }
        
        /* Horizontal scan lines */
        body::before {
            content: '';
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: 
                repeating-linear-gradient(
                    0deg,
                    rgba(0, 0, 0, 0.15),
                    rgba(0, 0, 0, 0.15) 1px,
                    transparent 1px,
                    transparent 2px
                );
            pointer-events: none;
            z-index: 1;
        }
        
        /* Falling glitch lines */
        body::after {
            content: '';
            position: fixed;
            top: -100%;
            left: 0;
            width: 100%;
            height: 100%;
    background-image:
    linear-gradient(180deg,
        transparent 0%,
        transparent 10%,
        rgba(65, 255, 0, 0.03) 11%,
        rgba(65, 255, 0, 0.05) 12%,
        rgba(65, 255, 0, 0.03) 13%,
        transparent 14%,
        transparent 40%,
        rgba(65, 255, 0, 0.02) 41%,
        rgba(65, 255, 0, 0.04) 41.5%,
        rgba(65, 255, 0, 0.02) 42%,
        transparent 43%,
        transparent 65%,
        rgba(65, 255, 0, 0.03) 66%,
        rgba(65, 255, 0, 0.06) 67%,
        rgba(65, 255, 0, 0.03) 68%,
        transparent 69%,
        transparent 85%,
        rgba(65, 255, 0, 0.02) 86%,
        rgba(65, 255, 0, 0.03) 87%,
        transparent 88%,
        transparent 100%
    );
            animation: scanLineFall 8s linear infinite;
            pointer-events: none;
            z-index: 2;
            filter: blur(1px);
        }
        
        @keyframes scanLineFall {
            0% { 
                transform: translateY(0);
                opacity: 0;
            }
            5% {
                opacity: 1;
            }
            95% {
                opacity: 1;
            }
            100% { 
                transform: translateY(200%);
                opacity: 0;
            }
        }
        
        .div-center-framed-content {
            font-family: 'VT323', 'Courier New', monospace !important;
            font-size: 12pt !important;
            background: #0a0a0a;
            color: #b3b3b3;
            font-family: 'VT323', monospace;
            border: 2px solid #41ff00;
            border-style: double;
            border-width: 4px;
            box-shadow: 
                inset 0 0 50px rgba(65, 255, 0, 0.1),
                0 0 20px rgba(65, 255, 0, 0.3);
            position: relative;
            z-index: 10;
        }

        .div-center-framed-content, .div-center-framed-content span, .div-center-framed-content td, .div-center-framed-content button, .div-center-framed-content input {
            font-family: 'VT323', 'Courier New', monospace !important;
            font-size: 12pt;
        }
        
        /* CRT curve effect */
        .div-center-framed-content::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: radial-gradient(
                ellipse at center,
                transparent 0%,
                rgba(0, 0, 0, 0.4) 100%
            );
            pointer-events: none;
        }
        
        /* Phosphor burn-in effect */
        .div-center-framed-content::after {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: 
                repeating-linear-gradient(
                    90deg,
                    transparent,
                    transparent 2px,
                    rgba(65, 255, 0, 0.03) 2px,
                    rgba(65, 255, 0, 0.03) 4px
                );
            pointer-events: none;
            animation: phosphorFlicker 0.05s infinite;
        }
        
        @keyframes phosphorFlicker {
            0% { opacity: 0.95; }
            50% { opacity: 1; }
            100% { opacity: 0.98; }
        }
        
        .div-center-framed-content h1 {
            color: #41ff00;
            text-align: center;
            font-size: 36px !important;
            letter-spacing: 2px;
            text-shadow: 
                0 0 5px #41ff00,
                0 0 10px #41ff00,
                0 0 15px #41ff00;
            animation: textGlow 2s ease-in-out infinite alternate;
        }
        
        @keyframes textGlow {
            from { text-shadow: 0 0 5px #41ff00, 0 0 10px #41ff00; }
            to { text-shadow: 0 0 10px #41ff00, 0 0 20px #41ff00, 0 0 30px #41ff00; }
        }
        
        input[type="file"] {
            margin: 0 0 20px 10px;
            padding: 8px;
            border: 1px dashed #41ff00;
            background: #000;
            color: #41ff00;
            font-size: 18px !important;
            cursor: pointer;
            transition: all 0.3s ease;
            text-transform: uppercase;
        }

        input[type="file"]:hover {
            background: rgba(65, 255, 0, 0.1);
            box-shadow: 0 0 10px rgba(65, 255, 0, 0.5);
        }
        
        input[type="file"]::file-selector-button {
            background: #000;
            color: #41ff00;
            border: 1px solid #41ff00;
            padding: 4px 8px;
            font-family: 'VT323', monospace;
        }
        
        input[type=submit], button, .buttonmain, a.buttonmain {
            background: #000;
            border: 1px solid #41ff00;
            color: #41ff00;
            font-size: 20px !important;
            text-transform: uppercase;
            position: relative;
            box-shadow: inset 0 0 0 0 #41ff00;
            transition: all 0.3s;
            text-shadow: 0 0 5px #41ff00;
        }
        
        input[type=submit]:hover, button:hover, .buttonmain:hover, a.buttonmain:hover {
            background: #41ff00;
            color: #000;
            box-shadow: 
                inset 0 0 10px #000,
                0 0 20px #41ff00;
            text-shadow: none;
            animation: buttonBlink 0.2s infinite;
        }
        
        @keyframes buttonBlink {
            0%, 100% { opacity: 1; }
            50% { opacity: 0.8; }
        }

        #progress_bar_container {
            background: #000;
            border: 1px solid #41ff00;
            height: 30px;
            position: relative;
            overflow: hidden;
            margin: 20px 0;
            box-shadow: inset 0 0 10px rgba(0, 0, 0, 0.8);
        }
        
        /* ASCII-style progress markers */
        #progress_bar_container::after {
            content: '|--------10--------20--------30--------40--------50--------60--------70--------80--------90-------|';
            position: absolute;
            top: 50%;
            left: 0;
            right: 0;
            transform: translateY(-50%);
            color: #41ff00;
            opacity: 0.3;
            font-size: 12px;
            text-align: center;
            letter-spacing: 0;
            pointer-events: none;
        }

        #progress_bar_indicator {
            background: #41ff00;
            height: 100%;
            width: 0%;
            transition: width 0.3s ease;
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
            box-shadow: 
                0 0 10px #41ff00,
                0 0 20px #41ff00,
                inset 0 0 10px rgba(255, 255, 255, 0.5);
        }
        
        /* Animated block characters */
        #progress_bar_indicator::before {
            content: '████████████████████████████████████████████████████████████████████████████████';
            position: absolute;
            left: 0;
            color: rgba(0, 0, 0, 0.3);
            overflow: hidden;
            white-space: nowrap;
            animation: blockScroll 2s linear infinite;
        }
        
        @keyframes blockScroll {
            0% { transform: translateX(0); }
            100% { transform: translateX(-50%); }
        }

        #labelPercent {
            color: #000;
            background: #41ff00;
            padding: 2px 6px;
            font-weight: normal;
            font-size: 20px !important;
            z-index: 10;
            border: 1px solid #000;
            animation: percentBlink 1s infinite;
        }
        
        @keyframes percentBlink {
            0%, 90% { opacity: 1; }
            95% { opacity: 0.3; }
            100% { opacity: 1; }
        }

        .div_task_status {
            position: relative;
            margin-top: 20px;
            padding: 20px;
            background: #000;
            border: 1px solid #41ff00;
            font-size: 12pt;
        }

        .div_task_status::before {
            content: '╔═══════════════════════════════════════════════════════════════════════════════════════════════╗';
            display: block;
            color: #41ff00;
            margin-bottom: 10px;
        }

        .div_task_status::after {
            content: '╚═══════════════════════════════════════════════════════════════════════════════════════════════╝';
            display: block;
            color: #41ff00;
            margin-top: 10px;
        }

        .div_task_status table {
            border-collapse: collapse;
            font-size: 18px !important;
            line-height: 1.4;
            color: #41ff00;
        }

        .div_task_status td {
            padding: 6px 15px;
            border: none;
            vertical-align: top;
            color: #41ff00;
            text-shadow: 0 0 5px #41ff00;
            font-size: 12pt;
        }

        .div_task_status td:first-child {
            font-weight: normal;
            color: #41ff00;
            background: none;
            border-right: none;
            width: 200px;
            text-align: left;
            text-transform: uppercase;
        }
        
        .div_task_status td:first-child::before {
            content: '▶ ';
            animation: caretBlink 1s infinite;
        }
        
        @keyframes caretBlink {
            0%, 49% { opacity: 1; }
            50%, 100% { opacity: 0; }
        }

        .div_task_status td:nth-child(2) {
            background: none;
            min-width: 150px;
        }
        
        .div_task_status td:nth-child(2)::before {
            content: ': ';
        }

        .div_task_status tr {
            border-bottom: 1px dashed #41ff00;
        }

        .div_task_status tr:last-child {
            border-bottom: none;
        }

        .status-complete {
            animation: statusComplete 0.5s infinite !important;
        }
        
        .status-complete::before {
            content: '[OK] ';
        }

        .status-running {
            animation: statusRunning 0.3s infinite !important;
        }
        
        .status-running::before {
            content: '[>>>] ';
            animation: runningArrows 1s infinite;
        }
        
        @keyframes runningArrows {
            0% { content: '[>  ] '; }
            33% { content: '[>> ] '; }
            66% { content: '[>>>] '; }
            100% { content: '[>  ] '; }
        }

        .status-error {
            color: #41ff00 !important;
            animation: errorFlash 0.2s infinite !important;
        }
        
        .status-error::before {
            content: '[ERR] ';
        }

        @keyframes statusComplete {
            0%, 100% { opacity: 1; }
            50% { opacity: 0.8; }
        }

        @keyframes statusRunning {
            0%, 100% { text-shadow: 0 0 5px #41ff00; }
            50% { text-shadow: 0 0 15px #41ff00, 0 0 25px #41ff00; }
        }

        @keyframes errorFlash {
            0%, 100% { 
                background: #41ff00; 
                color: #000 !important;
                padding: 0 5px;
            }
            50% { 
                background: #000; 
                color: #41ff00 !important;
                padding: 0 5px;
            }
        }
        
        /* Links in terminal theme */
        .div_task_status a, 
        .div_task_status a.mainbutton {
            color: #41ff00;
            background: #000;
            border: 1px solid #41ff00;
            text-decoration: underline;
            text-transform: uppercase;
            padding: 2px 8px;
            display: inline-block;
        }
        
        .div_task_status a:hover,
        .div_task_status a.mainbutton:hover {
            background: #41ff00;
            color: #000;
            text-decoration: none;
            animation: linkBlink 0.1s infinite;
        }
        
        @keyframes linkBlink {
            0%, 100% { opacity: 1; }
            50% { opacity: 0.7; }
        }
        
        /* Additional terminal elements */
        span {
            color: #41ff00;
            text-shadow: 0 0 3px #41ff00;
        }
        
        /* Terminal cursor effect */
        *:focus {
            outline: 1px solid #41ff00;
            outline-offset: 2px;
            box-shadow: 0 0 10px #41ff00;
        }
        
        /* Old CRT monitor effect */
        @keyframes crtFlicker {
            0% { opacity: 0.97; }
            10% { opacity: 1; }
            20% { opacity: 0.95; }
            30% { opacity: 1; }
            40% { opacity: 0.98; }
            50% { opacity: 1; }
            60% { opacity: 0.96; }
            70% { opacity: 0.99; }
            80% { opacity: 1; }
            90% { opacity: 0.97; }
            100% { opacity: 1; }
        }
        
        .div-center-framed-content {
            animation: crtFlicker 10s infinite;
        }
    `;
}

function loadThemeSteampunkVictorian() {
    styleTheme.innerText = `

    /* Steampunk Victorian Theme - Enhanced Brass, Copper, Gears & Ornaments */
@import url('https://fonts.googleapis.com/css2?family=Cinzel:wght@400;600&family=Crimson+Text:ital@0;1&display=swap');

body {
    background: #1a1410;
    background-image:
        radial-gradient(circle at 20% 50%, rgba(184, 134, 11, 0.3) 0%, transparent 50%),
        radial-gradient(circle at 80% 80%, rgba(139, 69, 19, 0.3) 0%, transparent 50%),
        radial-gradient(circle at 40% 20%, rgba(205, 127, 50, 0.2) 0%, transparent 40%);
    position: relative;
}

/* Enhanced gear pattern overlay with intricate Victorian design */
body::before {
    content: '';
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-image:
        radial-gradient(circle at 100% 0%, transparent 20%, rgba(184, 134, 11, 0.1) 21%, transparent 22%),
        radial-gradient(circle at 0% 100%, transparent 20%, rgba(139, 69, 19, 0.1) 21%, transparent 22%),
        repeating-linear-gradient(
            45deg,
            rgba(205, 127, 50, 0.05),
            rgba(205, 127, 50, 0.05) 10px,
            transparent 10px,
            transparent 20px
        );
    background-size: 100px 100px;
    pointer-events: none;
    opacity: 0.6;
}

.div-center-framed-content {
    color: #c78f36;
    font-family: 'Cinzel', serif;
    background: linear-gradient(135deg, #2a2218 0%, #1a1410 100%);
    border: 4px double #b8860b;
    border-radius: 5px;
    box-shadow:
        0 0 40px rgba(184, 134, 11, 0.5),
        inset 0 0 40px rgba(0, 0, 0, 0.6),
        inset 0 0 120px rgba(184, 134, 11, 0.15);
    position: relative;
    overflow: visible;
}

/* Ornate corner decorations with Victorian flourishes */
.div-center-framed-content::before,
.div-center-framed-content::after {
    content: '⚙❧';
    position: absolute;
    font-size: 50px;
    color: #b8860b;
    text-shadow:
        0 0 15px rgba(184, 134, 11, 0.6),
        0 3px 6px rgba(0, 0, 0, 0.9);
    animation: gearRotate 20s linear infinite;
}

.div-center-framed-content::before {
    top: -30px;
    left: -30px;
    transform: rotate(-10deg);
}

.div-center-framed-content::after {
    bottom: -30px;
    right: -30px;
    transform: rotate(10deg);
    animation-direction: reverse;
}

@keyframes gearRotate {
    from { transform: rotate(0deg); }
    to { transform: rotate(360deg); }
}

.div-center-framed-content h1 {
    font-family: 'Cinzel', serif;
    color: #b8860b;
    text-align: center;
    font-size: 40px !important;
    font-weight: 600;
    letter-spacing: 4px;
    text-shadow:
        0 3px 6px rgba(0, 0, 0, 0.9),
        0 0 25px rgba(184, 134, 11, 0.5);
    text-transform: uppercase;
    position: relative;
    padding: 25px 0;
}

/* Enhanced Victorian ornamentation */
.div-center-framed-content h1::before,
.div-center-framed-content h1::after {
    content: '❦✿';
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    font-size: 28px;
    color: #cd7f32;
    text-shadow: 0 1px 3px rgba(0, 0, 0, 0.8);
}

.div-center-framed-content h1::before {
    left: 25px;
}

.div-center-framed-content h1::after {
    right: 25px;
}

input[type="file"] {
    margin: 0 0 25px 15px;
    padding: 12px;
    border: 3px solid #cd7f32;
    background: linear-gradient(135deg, #3a2818 0%, #2a1810 100%);
    color: #daa520;
    font-family: 'Crimson Text', serif;
    font-size: 18px !important;
    cursor: pointer;
    transition: all 0.4s ease;
    box-shadow:
        inset 0 2px 0 rgba(255, 255, 255, 0.15),
        0 3px 6px rgba(0, 0, 0, 0.6);
    border-radius: 5px;
}

input[type="file"]:hover {
    background: linear-gradient(135deg, #4a3828 0%, #3a2818 100%);
    box-shadow:
        inset 0 2px 0 rgba(255, 255, 255, 0.25),
        0 3px 10px rgba(184, 134, 11, 0.6);
    border-color: #daa520;
}

input[type="file"]::file-selector-button {
    background: linear-gradient(135deg, #3a2818 0%, #2a1810 100%);
    color: #daa520;
    border: 2px solid #cd7f32;
    padding: 6px 12px;
    font-family: 'Crimson Text', serif;
    border-radius: 3px;
}

input[type=submit], button, .buttonmain, a.buttonmain {
    font-family: 'Cinzel', serif;
    background: linear-gradient(135deg, #b8860b 0%, #8b6914 50%, #b8860b 100%);
    border: 3px solid #cd7f32;
    color: #1a1410;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 2px;
    position: relative;
    box-shadow:
        inset 0 2px 0 rgba(255, 255, 255, 0.4),
        inset 0 -2px 0 rgba(0, 0, 0, 0.4),
        0 5px 10px rgba(0, 0, 0, 0.6);
    transition: all 0.4s;
    text-shadow: 0 2px 0 rgba(255, 255, 255, 0.4);
    border-radius: 5px;
}

input[type=submit]:hover, button:hover, .buttonmain:hover, a.buttonmain:hover {
    background: linear-gradient(135deg, #daa520 0%, #b8860b 50%, #daa520 100%);
    box-shadow:
        inset 0 2px 0 rgba(255, 255, 255, 0.5),
        inset 0 -2px 0 rgba(0, 0, 0, 0.5),
        0 5px 15px rgba(184, 134, 11, 0.7);
    transform: translateY(-2px);
}

input[type=submit]:active, button:active {
    transform: translateY(2px);
    box-shadow:
        inset 0 2px 0 rgba(255, 255, 255, 0.2),
        inset 0 -2px 0 rgba(0, 0, 0, 0.2),
        0 2px 5px rgba(0, 0, 0, 0.6);
}

#progress_bar_container {
    background: linear-gradient(90deg, #1a1410 0%, #2a2218 50%, #1a1410 100%);
    border: 3px double #8b6914;
    border-radius: 20px;
    height: 50px;
    position: relative;
    overflow: hidden;
    margin: 25px 0;
    box-shadow:
        inset 0 3px 10px rgba(0, 0, 0, 0.9),
        0 2px 0 rgba(184, 134, 11, 0.4);
}

/* Enhanced rivet decoration with Victorian flair */
#progress_bar_container::before,
#progress_bar_container::after {
    content: '◉ ◉ ◉ ◉ ◉ ◉ ◉ ◉ ◉ ◉';
    position: absolute;
    left: 0;
    right: 0;
    color: #8b6914;
    font-size: 10px;
    letter-spacing: 30px;
    text-align: center;
    text-shadow: 0 2px 3px rgba(0, 0, 0, 0.9);
}

#progress_bar_container::before {
    top: 3px;
}

#progress_bar_container::after {
    bottom: 3px;
}

#progress_bar_indicator {
    background: linear-gradient(90deg,
        #cd7f32 0%,
        #daa520 25%,
        #b8860b 50%,
        #daa520 75%,
        #cd7f32 100%);
    background-size: 250% 100%;
    height: 100%;
    border-radius: 18px;
    width: 0%;
    transition: width 0.6s ease;
    display: flex;
    align-items: center;
    justify-content: right;
    position: relative;
    animation: brassShine 3.5s linear infinite;
    box-shadow:
        inset 0 3px 6px rgba(255, 255, 255, 0.4),
        inset 0 -3px 6px rgba(0, 0, 0, 0.4),
        0 0 25px rgba(184, 134, 11, 0.7);
}

@keyframes brassShine {
    0% { background-position: 0% 50%; }
    100% { background-position: 250% 50%; }
}

/* Mechanical gauge with Victorian engraving effect */
#progress_bar_indicator::after {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: repeating-linear-gradient(
        90deg,
        transparent,
        transparent 6px,
        rgba(0, 0, 0, 0.15) 6px,
        rgba(0, 0, 0, 0.15) 12px
    );
    border-radius: 18px;
}

#labelPercent {
    color: #1a1410;
    background: radial-gradient(circle, #daa520 0%, #b8860b 100%);
    padding: 6px 15px;
    font-family: 'Cinzel', serif;
    font-weight: 600;
    font-size: 18px !important;
    z-index: 10;
    border: 2px solid #8b6914;
    border-radius: 25px;
    box-shadow:
        inset 0 2px 0 rgba(255, 255, 255, 0.5),
        0 3px 6px rgba(0, 0, 0, 0.6);
    text-shadow: 0 2px 0 rgba(255, 255, 255, 0.4);
}

.div_task_status {
    margin-top: 25px;
    padding: 25px;
    background: linear-gradient(135deg, #2a2218 0%, #1a1410 100%);
    border: 3px double #8b6914;
    box-shadow:
        inset 0 0 25px rgba(0, 0, 0, 0.6),
        0 5px 10px rgba(0, 0, 0, 0.6);
    position: relative;
}

/* Decorative frame corners with Victorian scrollwork */
.div_task_status::before,
.div_task_status::after {
    content: '';
    position: absolute;
    width: 50px;
    height: 50px;
    border: 3px solid #b8860b;
    background: radial-gradient(circle, #b8860b 0%, transparent 70%);
}

.div_task_status::before {
    top: -3px;
    left: -3px;
    border-right: none;
    border-bottom: none;
    border-top-left-radius: 15px;
}

.div_task_status::after {
    bottom: -3px;
    right: -3px;
    border-left: none;
    border-top: none;
    border-bottom-right-radius: 15px;
}

.div_task_status table {
    width: 100%;
    border-collapse: separate;
    border-spacing: 0;
    font-family: 'Crimson Text', serif;
    font-size: 20px !important;
    line-height: 1.8;
    color: #daa520;
}

.div_task_status td {
    padding: 5px 25px;
    border-bottom: 2px dashed rgba(184, 134, 11, 0.4);
    vertical-align: middle;
    color: #daa520;
    background: rgba(0, 0, 0, 0.25);
}

.div_task_status td:first-child {
    font-family: 'Cinzel', serif;
    font-weight: 600;
    color: #cd7f32;
    background: linear-gradient(90deg,
        rgba(139, 69, 19, 0.4) 0%,
        transparent 100%);
    border-right: 3px solid #8b6914;
    width: 240px;
    text-align: right;
    text-transform: uppercase;
    letter-spacing: 2px;
    font-size: 16px !important;
    text-shadow: 0 2px 3px rgba(0, 0, 0, 0.9);
}

.div_task_status td:nth-child(2) {
    background: rgba(184, 134, 11, 0.1);
    font-style: italic;
    padding-left: 35px;
}

.div_task_status tr:last-child td {
    border-bottom: none;
}

.status-complete {
    color: #90ee90 !important;
    text-shadow: 0 0 15px rgba(144, 238, 144, 0.6);
    position: relative;
    padding-left: 30px !important;
}

.status-complete::before {
    content: '✓✿';
    position: absolute;
    left: 5px;
    font-size: 22px;
    color: #90ee90;
}

.status-running {
    color: #ffd700 !important;
    text-shadow: 0 0 15px rgba(255, 215, 0, 0.6);
    position: relative;
    padding-left: 30px !important;
    animation: steamPuff 2.5s ease-in-out infinite;
}

.status-running::before {
    content: '⚙❖';
    position: absolute;
    left: 5px;
    font-size: 18px;
    animation: gearSpin 2.5s linear infinite;
}

@keyframes gearSpin {
    from { transform: rotate(0deg); }
    to { transform: rotate(360deg); }
}

@keyframes steamPuff {
    0%, 100% { opacity: 1; }
    50% { opacity: 0.6; }
}

.status-error {
    color: #dc143c !important;
    text-shadow: 0 0 15px rgba(220, 20, 60, 0.6);
    position: relative;
    padding-left: 30px !important;
}

.status-error::before {
    content: '⚠❗';
    position: absolute;
    left: 5px;
    font-size: 20px;
    color: #dc143c;
}

.div_task_status a,
.div_task_status a.mainbutton {
    color: #daa520;
    background: linear-gradient(135deg, #8b6914 0%, #b8860b 100%);
    border: 2px solid #cd7f32;
    text-decoration: none;
    padding: 6px 15px;
    display: inline-block;
    font-family: 'Cinzel', serif;
    font-weight: 600;
    text-transform: uppercase;
    letter-spacing: 2px;
    font-size: 14px !important;
    box-shadow:
        inset 0 2px 0 rgba(255, 255, 255, 0.3),
        0 3px 6px rgba(0, 0, 0, 0.6);
    border-radius: 5px;
    transition: all 0.4s;
}

.div_task_status a:hover,
.div_task_status a.mainbutton:hover {
    background: linear-gradient(135deg, #b8860b 0%, #daa520 100%);
    color: #1a1410;
    text-decoration: none;
    box-shadow:
        inset 0 2px 0 rgba(255, 255, 255, 0.4),
        0 3px 10px rgba(184, 134, 11, 0.7);
    transform: translateY(-2px);
}

span {
    color: #daa520;
}

/* Victorian ornament divider with enhanced flourish */
h1 + *::before {
    content: '〜 ❦ ✿ 〜';
    position: absolute;
    top: -25px;
    left: 50%;
    transform: translateX(-50%);
    color: #8b6914;
    font-size: 18px;
    text-shadow: 0 2px 3px rgba(0, 0, 0, 0.9);
}

/* Copper pipe effect with Victorian engraving */
input:focus, button:focus, textarea:focus, select:focus {
    outline: none;
    border-color: #daa520;
    box-shadow:
        0 0 25px rgba(218, 165, 32, 0.5),
        inset 0 0 15px rgba(218, 165, 32, 0.15);
    border-style: double;
}

    `;
}

function loadThemeSolarFire() {
    styleTheme.innerText = `
        /* Solar/Fire Theme - Flames, Heat & Molten Energy */
        @import url('https://fonts.googleapis.com/css2?family=Bebas+Neue&family=Oswald:wght@400;600&display=swap');
        
        body {
            background: #0a0000;
            background-image: 
                radial-gradient(ellipse at bottom, #330000 0%, transparent 70%),
                radial-gradient(ellipse at top, #1a0500 0%, transparent 70%),
                linear-gradient(180deg, #0a0000 0%, #1a0000 100%);
            position: relative;
            overflow-x: hidden;
        }
        
        /* Animated embers floating up */
        body::before, body::after {
            content: '';
            position: fixed;
            width: 100%;
            height: 100%;
            pointer-events: none;
        }
        
        body::before {
            background-image: 
                radial-gradient(circle, #ff6600 0%, transparent 70%),
                radial-gradient(circle, #ff3300 0%, transparent 70%),
                radial-gradient(circle, #ffaa00 0%, transparent 70%);
            background-size: 3px 3px, 4px 4px, 2px 2px;
            background-position: 20% 100%, 60% 100%, 80% 100%;
            animation: embersFloat 20s linear infinite;
            opacity: 0.6;
        }
        
        @keyframes embersFloat {
            0% { transform: translateY(100vh); }
            100% { transform: translateY(-100vh); }
        }
        
        .div-center-framed-content {
            background: linear-gradient(135deg, #1a0000 0%, #330000 50%, #1a0000 100%);
            border: 2px solid #ff3300;
            border-radius: 0;
            box-shadow: 
                0 0 50px rgba(255, 51, 0, 0.5),
                inset 0 0 50px rgba(255, 102, 0, 0.2),
                0 0 100px rgba(255, 102, 0, 0.3);
            position: relative;
            overflow: hidden;
        }
        
        /* Heat shimmer effect */
        .div-center-framed-content::before {
            content: '';
            position: absolute;
            top: -50%;
            left: -50%;
            width: 200%;
            height: 200%;
            background: linear-gradient(45deg, 
                transparent 40%, 
                rgba(255, 102, 0, 0.1) 50%, 
                transparent 60%);
            animation: heatShimmer 3s linear infinite;
        }
        
        @keyframes heatShimmer {
            0% { transform: translate(0, 0); }
            100% { transform: translate(50%, 50%); }
        }
        
        /* Flame border effect */
        .div-center-framed-content::after {
            content: '';
            position: absolute;
            top: -2px;
            left: -2px;
            right: -2px;
            bottom: -2px;
            background: linear-gradient(45deg, 
                #ff0000, #ff3300, #ff6600, #ffaa00, #ffcc00,
                #ffaa00, #ff6600, #ff3300, #ff0000);
            background-size: 400% 400%;
            z-index: -1;
            animation: flameBorder 2s linear infinite;
            filter: blur(3px);
            opacity: 0.8;
        }
        
        @keyframes flameBorder {
            0% { background-position: 0% 50%; }
            100% { background-position: 100% 50%; }
        }
        
        .div-center-framed-content h1 {
            font-family: 'Bebas Neue', sans-serif;
            color: #ff6600;
            text-align: center;
            font-size: 48px !important;
            font-weight: 400;
            letter-spacing: 4px;
            text-transform: uppercase;
            text-shadow: 
                0 0 10px #ff3300,
                0 0 20px #ff0000,
                0 0 30px #cc0000,
                0 -2px 10px #ffaa00;
            animation: fireText 2s ease-in-out infinite alternate;
            position: relative;
        }
        
        @keyframes fireText {
            0% { 
                text-shadow: 
                    0 0 10px #ff3300,
                    0 0 20px #ff0000,
                    0 0 30px #cc0000,
                    0 -2px 10px #ffaa00;
                transform: scale(1);
            }
            100% { 
                text-shadow: 
                    0 0 20px #ff6600,
                    0 0 40px #ff3300,
                    0 0 60px #ff0000,
                    0 -4px 20px #ffcc00;
                transform: scale(1.02);
            }
        }
        
        input[type="file"] {
            margin: 0 0 20px 10px;
            padding: 10px;
            border: 1px solid #ff6600;
            background: linear-gradient(135deg, #330000 0%, #1a0000 100%);
            color: #ffaa00;
            font-family: 'Oswald', sans-serif;
            font-size: 14px !important;
            cursor: pointer;
            transition: all 0.3s ease;
            text-transform: uppercase;
            position: relative;
            overflow: hidden;
        }

        input[type="file"]:hover {
            background: linear-gradient(135deg, #660000 0%, #330000 100%);
            box-shadow: 
                0 0 20px rgba(255, 102, 0, 0.6),
                inset 0 0 20px rgba(255, 102, 0, 0.2);
            border-color: #ff9900;
        }

        input[type="file"]::file-selector-button {
            padding: 10px;
            border: 1px solid #ff6600;
            background: linear-gradient(135deg, #330000 0%, #1a0000 100%);
            color: #ffaa00;
            font-family: 'Oswald', sans-serif;
            font-size: 14px !important;
            cursor: pointer;
        }

        input[type=submit], button, .buttonmain, a.buttonmain {
            font-family: 'Bebas Neue', sans-serif;
            background: linear-gradient(135deg, #cc0000 0%, #ff3300 50%, #ff6600 100%);
            border: none;
            color: #ffcc00;
            font-size: 18px !important;
            text-transform: uppercase;
            letter-spacing: 2px;
            position: relative;
            overflow: hidden;
            transition: all 0.3s;
            text-shadow: 0 2px 4px rgba(0, 0, 0, 0.8);
            box-shadow: 
                0 4px 15px rgba(255, 51, 0, 0.4),
                inset 0 -2px 10px rgba(0, 0, 0, 0.4);
        }
        
        input[type=submit]::before, button::before {
            content: '';
            position: absolute;
            top: -50%;
            left: -50%;
            width: 200%;
            height: 200%;
            background: radial-gradient(circle, rgba(255, 255, 0, 0.3) 0%, transparent 70%);
            transform: scale(0);
            transition: transform 0.5s;
        }
        
        input[type=submit]:hover::before, button:hover::before {
            transform: scale(1);
        }
        
        input[type=submit]:hover, button:hover, .buttonmain:hover, a.buttonmain:hover {
            background: linear-gradient(135deg, #ff0000 0%, #ff6600 50%, #ff9900 100%);
            color: #fff;
            box-shadow: 
                0 4px 25px rgba(255, 102, 0, 0.6),
                inset 0 -2px 15px rgba(0, 0, 0, 0.5);
            transform: translateY(-2px);
            text-shadow: 
                0 0 10px rgba(255, 255, 255, 0.8),
                0 2px 4px rgba(0, 0, 0, 0.8);
        }

        #progress_bar_container {
            background: linear-gradient(90deg, #0a0000 0%, #1a0000 50%, #0a0000 100%);
            border: 1px solid #660000;
            border-radius: 25px;
            height: 40px;
            position: relative;
            overflow: hidden;
            margin: 20px 0;
            box-shadow: 
                inset 0 0 20px rgba(0, 0, 0, 0.8),
                0 0 30px rgba(255, 51, 0, 0.3);
        }
        
        /* Molten lava texture */
        #progress_bar_container::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: 
                repeating-linear-gradient(90deg, 
                    transparent 0px, 
                    rgba(255, 51, 0, 0.1) 2px, 
                    transparent 4px, 
                    transparent 20px),
                repeating-linear-gradient(0deg, 
                    transparent 0px, 
                    rgba(255, 102, 0, 0.1) 2px, 
                    transparent 4px, 
                    transparent 20px);
            animation: lavaFlow 10s linear infinite;
        }
        
        @keyframes lavaFlow {
            0% { transform: translate(0, 0); }
            100% { transform: translate(20px, 20px); }
        }

        #progress_bar_indicator {
            background: linear-gradient(90deg, 
                #ff0000 0%, 
                #ff3300 20%, 
                #ff6600 40%, 
                #ff9900 50%, 
                #ffcc00 60%, 
                #ff9900 70%, 
                #ff6600 80%, 
                #ff3300 90%, 
                #ff0000 100%);
            background-size: 200% 100%;
            height: 100%;
            border-radius: 25px;
            width: 0%;
            transition: width 0.5s ease;
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
            animation: fireGradient 3s linear infinite, moltenPulse 2s ease-in-out infinite;
            box-shadow: 
                0 0 30px rgba(255, 102, 0, 0.8),
                inset 0 0 20px rgba(255, 255, 0, 0.4);
            filter: brightness(1.2) contrast(1.1);
        }
        
        @keyframes fireGradient {
            0% { background-position: 0% 50%; }
            100% { background-position: 200% 50%; }
        }
        
        @keyframes moltenPulse {
            0%, 100% { 
                filter: brightness(1.2) contrast(1.1);
                box-shadow: 
                    0 0 30px rgba(255, 102, 0, 0.8),
                    inset 0 0 20px rgba(255, 255, 0, 0.4);
            }
            50% { 
                filter: brightness(1.4) contrast(1.2);
                box-shadow: 
                    0 0 50px rgba(255, 102, 0, 1),
                    inset 0 0 30px rgba(255, 255, 0, 0.6);
            }
        }
        
        /* Flame particles inside progress */
        #progress_bar_indicator::after {
            content: '🔥';
            position: absolute;
            right: 10px;
            font-size: 20px;
            animation: flameFlicker 0.5s infinite alternate;
        }
        
        @keyframes flameFlicker {
            0% { transform: scale(1) rotate(-5deg); opacity: 0.8; }
            100% { transform: scale(1.2) rotate(5deg); opacity: 1; }
        }

        #labelPercent {
            color: #000;
            background: radial-gradient(circle, #ffcc00 0%, #ff9900 50%, #ff6600 100%);
            padding: 4px 12px;
            font-family: 'Bebas Neue', sans-serif;
            font-weight: 400;
            font-size: 20px !important;
            z-index: 10;
            border: 1px solid #ff3300;
            border-radius: 20px;
            box-shadow: 
                0 0 20px rgba(255, 153, 0, 0.8),
                inset 0 0 10px rgba(255, 255, 255, 0.4);
            text-shadow: 0 1px 2px rgba(0, 0, 0, 0.6);
            animation: percentGlow 1s ease-in-out infinite alternate;
        }
        
        @keyframes percentGlow {
            0% { box-shadow: 0 0 20px rgba(255, 153, 0, 0.8); }
            100% { box-shadow: 0 0 30px rgba(255, 204, 0, 1); }
        }

        .div_task_status {
            margin-top: 20px;
            padding: 20px;
            background: linear-gradient(135deg, #1a0000 0%, #330000 100%);
            border: 1px solid #ff3300;
            box-shadow: 
                inset 0 0 30px rgba(255, 51, 0, 0.2),
                0 0 40px rgba(255, 51, 0, 0.4);
            position: relative;
            overflow: hidden;
        }
        
        /* Flame animation top border */
        .div_task_status::before {
            content: '';
            position: absolute;
            top: -10px;
            left: 0;
            right: 0;
            height: 20px;
            background: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 20"><path d="M0,20 Q10,0 20,20 T40,20 T60,20 T80,20 T100,20" fill="%23ff6600" opacity="0.8"/></svg>');
            background-size: 100px 20px;
            animation: fireWave 2s linear infinite;
            filter: blur(2px);
        }
        
        @keyframes fireWave {
            0% { transform: translateX(0); }
            100% { transform: translateX(-100px); }
        }

        .div_task_status table {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0 5px;
            font-family: 'Oswald', sans-serif;
            font-size: 16px !important;
            line-height: 1.6;
            color: #ffaa00;
        }

        .div_task_status td {
            padding: 6px 20px;
            border: none;
            vertical-align: middle;
            color: #ffaa00;
            background: linear-gradient(90deg, rgba(51, 0, 0, 0.5) 0%, rgba(26, 0, 0, 0.5) 100%);
            border-left: 3px solid transparent;
            transition: all 0.3s;
        }
        
        .div_task_status tr:hover td {
            border-left-color: #ff6600;
            background: linear-gradient(90deg, rgba(102, 0, 0, 0.5) 0%, rgba(51, 0, 0, 0.5) 100%);
            text-shadow: 0 0 10px rgba(255, 170, 0, 0.5);
        }

        .div_task_status td:first-child {
            font-family: 'Bebas Neue', sans-serif;
            font-weight: 400;
            color: #ff6600;
            background: linear-gradient(90deg, 
                rgba(102, 0, 0, 0.4) 0%, 
                transparent 100%);
            border-right: 1px solid #660000;
            width: 180px;
            text-align: right;
            text-transform: uppercase;
            letter-spacing: 2px;
            font-size: 16px;
            text-shadow: 0 0 10px rgba(255, 102, 0, 0.8);
        }

        .div_task_status td:nth-child(2) {
            background: rgba(255, 102, 0, 0.05);
            padding-left: 30px;
            font-weight: 400;
            font-size: 12px;
        }

        .status-complete {
            color: #00ff00 !important;
            text-shadow: 
                0 0 10px rgba(0, 255, 0, 0.8),
                0 0 20px rgba(0, 255, 0, 0.4);
            font-weight: 600;
            animation: successPulse 2s ease-in-out infinite;
        }
        
        @keyframes successPulse {
            0%, 100% { opacity: 1; }
            50% { opacity: 0.8; }
        }

        .status-running {
            color: #ffcc00 !important;
            text-shadow: 
                0 0 10px rgba(255, 204, 0, 0.8),
                0 0 20px rgba(255, 204, 0, 0.4);
            font-weight: 600;
            animation: burningText 1s ease-in-out infinite;
        }
        
        @keyframes burningText {
            0%, 100% { 
                text-shadow: 
                    0 0 10px rgba(255, 204, 0, 0.8),
                    0 0 20px rgba(255, 204, 0, 0.4);
            }
            50% { 
                text-shadow: 
                    0 0 20px rgba(255, 204, 0, 1),
                    0 0 30px rgba(255, 153, 0, 0.6);
            }
        }

        .status-error {
            color: #ff0000 !important;
            text-shadow: 
                0 0 10px rgba(255, 0, 0, 0.8),
                0 0 20px rgba(255, 0, 0, 0.4);
            font-weight: 600;
            animation: errorFlame 0.3s ease-in-out infinite;
        }
        
        @keyframes errorFlame {
            0%, 100% { transform: scale(1); }
            50% { transform: scale(1.05); }
        }
        
        /* Links in fire theme */
        .div_task_status a, 
        .div_task_status a.mainbutton {
            color: #000;
            background: linear-gradient(135deg, #ff6600 0%, #ff9900 100%);
            border: 1px solid #ff3300;
            text-decoration: none;
            padding: 4px 12px;
            display: inline-block;
            font-family: 'Bebas Neue', sans-serif;
            text-transform: uppercase;
            letter-spacing: 1px;
            font-size: 16px !important;
            transition: all 0.3s;
            box-shadow: 0 2px 10px rgba(255, 102, 0, 0.5);
        }
        
        .div_task_status a:hover,
        .div_task_status a.mainbutton:hover {
            background: linear-gradient(135deg, #ff9900 0%, #ffcc00 100%);
            color: #000;
            text-decoration: none;
            box-shadow: 
                0 2px 20px rgba(255, 153, 0, 0.8),
                0 0 30px rgba(255, 204, 0, 0.5);
            transform: translateY(-2px);
        }
        
        /* Additional fire elements */
        span {
            color: #ffaa00;
        }
        
        /* Solar flare effect */
        @keyframes solarFlare {
            0%, 100% { opacity: 0; }
            50% { opacity: 0.3; }
        }
        
        body::after {
            content: '';
            position: fixed;
            top: 50%;
            left: 50%;
            width: 200%;
            height: 200%;
            background: radial-gradient(circle, rgba(255, 255, 0, 0.2) 0%, transparent 50%);
            transform: translate(-50%, -50%);
            animation: solarFlare 4s ease-in-out infinite;
            pointer-events: none;
        }
    `;
}

function loadThemeFuturisticHUD() {
    styleTheme.innerText = `
        /* Futuristic HUD Terminal Theme - Advanced Holographic Interface */
        @import url('https://fonts.googleapis.com/css2?family=Orbitron:wght@400;700;900&family=Rajdhani:wght@300;400;600&display=swap');
        
        body {
            background: #000;
            background-image: 
                radial-gradient(ellipse at center, #001a33 0%, #000511 40%, #000 70%),
                repeating-linear-gradient(0deg, 
                    transparent, 
                    transparent 2px, 
                    rgba(0, 136, 204, 0.03) 2px, 
                    rgba(0, 136, 204, 0.03) 4px);
            position: relative;
            overflow-x: hidden;
        }
        
        /* Scanning line effect */
        body::before {
            content: '';
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 200px;
            background: linear-gradient(180deg, 
                transparent 0%, 
                rgba(0, 170, 255, 0.1) 50%, 
                transparent 100%);
            animation: scanDown 6s linear infinite;
            pointer-events: none;
        }
        
        @keyframes scanDown {
            0% { transform: translateY(-200px); }
            100% { transform: translateY(100vh); }
        }
        
        /* Grid overlay */
        body::after {
            content: '';
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-image: 
                linear-gradient(rgba(0, 136, 204, 0.1) 1px, transparent 1px),
                linear-gradient(90deg, rgba(0, 136, 204, 0.1) 1px, transparent 1px);
            background-size: 50px 50px;
            pointer-events: none;
            animation: gridPulse 4s ease-in-out infinite;
        }
        
        @keyframes gridPulse {
            0%, 100% { opacity: 0.3; }
            50% { opacity: 0.1; }
        }
        
        .div-center-framed-content {
            background: rgba(0, 11, 22, 0.8);
            border: 1px solid #0088cc;
            box-shadow: 
                0 0 50px rgba(0, 136, 204, 0.5),
                inset 0 0 50px rgba(0, 136, 204, 0.1),
                0 0 150px rgba(0, 170, 255, 0.2);
            position: relative;
            backdrop-filter: blur(10px);
            clip-path: polygon(
                0 20px, 20px 0,
                calc(100% - 20px) 0, 100% 20px,
                100% calc(100% - 20px), calc(100% - 20px) 100%,
                20px 100%, 0 calc(100% - 20px)
            );
            padding: 40px 30px;

            font-size: 12px;
        }
        
        /* Corner markers */
        .div-center-framed-content::before {
            content: '';
            position: absolute;
            top: 10px;
            left: 10px;
            right: 10px;
            bottom: 10px;
            border: 1px solid #0088cc;
            border-radius: 0;
            pointer-events: none;
            opacity: 0.5;
        }
        
        /* Animated corner brackets */
        .div-center-framed-content::after {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: 
                linear-gradient(90deg, #00aaff 30px, transparent 30px) top left,
                linear-gradient(180deg, #00aaff 30px, transparent 30px) top left,
                linear-gradient(270deg, #00aaff 30px, transparent 30px) top right,
                linear-gradient(180deg, #00aaff 30px, transparent 30px) top right,
                linear-gradient(90deg, #00aaff 30px, transparent 30px) bottom left,
                linear-gradient(0deg, #00aaff 30px, transparent 30px) bottom left,
                linear-gradient(270deg, #00aaff 30px, transparent 30px) bottom right,
                linear-gradient(0deg, #00aaff 30px, transparent 30px) bottom right;
            background-size: 2px 2px;
            background-repeat: no-repeat;
            pointer-events: none;
            animation: cornerGlow 3s ease-in-out infinite;
        }
        
        @keyframes cornerGlow {
            0%, 100% { opacity: 0.8; }
            50% { opacity: 1; filter: drop-shadow(0 0 10px #00aaff); }
        }
        
        .div-center-framed-content h1 {
            font-family: 'Orbitron', monospace;
            color: #00aaff;
            text-align: center;
            font-size: 32px !important;
            font-weight: 900;
            letter-spacing: 8px;
            text-transform: uppercase;
            text-shadow: 
                0 0 20px rgba(0, 170, 255, 0.8),
                0 0 40px rgba(0, 170, 255, 0.4),
                0 0 60px rgba(0, 170, 255, 0.2);
            margin: 30px 0;
            position: relative;
            animation: dataStream 0.1s infinite;
        }
        
        @keyframes dataStream {
            0% { opacity: 1; }
            50% { opacity: 0.98; }
            100% { opacity: 1; }
        }
        
        .div-center-framed-content h1::before,
        .div-center-framed-content h1::after {
            content: '';
            position: absolute;
            top: 50%;
            width: 100px;
            height: 1px;
            background: linear-gradient(90deg, transparent, #00aaff, transparent);
            animation: expandLine 3s ease-in-out infinite;
        }
        
        .div-center-framed-content h1::before {
            left: -120px;
        }
        
        .div-center-framed-content h1::after {
            right: -120px;
        }
        
        @keyframes expandLine {
            0%, 100% { width: 100px; opacity: 0.5; }
            50% { width: 150px; opacity: 1; }
        }
        
        input[type="file"] {
            margin: 0 0 20px 10px;
            padding: 10px 15px;
            border: 1px solid #0088cc;
            background: rgba(0, 136, 204, 0.1);
            color: #00aaff;
            font-family: 'Rajdhani', sans-serif;
            font-size: 16px !important;
            font-weight: 400;
            cursor: pointer;
            transition: all 0.3s ease;
            text-transform: uppercase;
            letter-spacing: 2px;
            position: relative;
            overflow: hidden;
        }

        input[type="file"]:hover {
            background: rgba(0, 136, 204, 0.2);
            box-shadow: 
                0 0 20px rgba(0, 170, 255, 0.6),
                inset 0 0 20px rgba(0, 170, 255, 0.1);
            border-color: #00aaff;
            text-shadow: 0 0 10px rgba(0, 170, 255, 0.8);
        }
        
        input[type="file"]::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(0, 170, 255, 0.4), transparent);
            transition: left 0.5s;
        }
        
        input[type="file"]:hover::before {
            left: 100%;
        }

        input[type="file"]::file-selector-button {
            border: 1px solid #0088cc;
            background: rgba(0, 136, 204, 0.1);
            color: #00aaff;
            font-family: 'Rajdhani', sans-serif;
            font-size: 16px !important;
            padding: 8px;
        }

        input[type=submit], button, .buttonmain, a.buttonmain {
            font-family: 'Orbitron', monospace;
            background: transparent;
            border: 1px solid #00aaff;
            color: #00aaff;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 3px;
            position: relative;
            overflow: hidden;
            transition: all 0.3s;
            clip-path: polygon(10px 0, 100% 0, 100% calc(100% - 10px), calc(100% - 10px) 100%, 0 100%, 0 10px);
            font-size: 11px;
        }
        
        input[type=submit]::before, button::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0, 170, 255, 0.1);
            transform: translateX(-100%);
            transition: transform 0.3s;
        }
        
        input[type=submit]:hover::before, button:hover::before {
            transform: translateX(0);
        }
        
        input[type=submit]:hover, button:hover, .buttonmain:hover, a.buttonmain:hover {
            color: #fff;
            border-color: #00d4ff;
            box-shadow: 
                0 0 30px rgba(0, 170, 255, 0.8),
                inset 0 0 20px rgba(0, 170, 255, 0.3);
            text-shadow: 0 0 10px rgba(0, 212, 255, 1);
            transform: translateY(-2px);
        }

        #progress_bar_container {
            background: linear-gradient(90deg, rgba(0, 11, 22, 0.9) 0%, rgba(0, 22, 44, 0.9) 50%, rgba(0, 11, 22, 0.9) 100%);
            border: 1px solid #0088cc;
            height: 40px;
            position: relative;
            overflow: hidden;
            margin: 30px 0;
            clip-path: polygon(20px 0, 100% 0, 100% 100%, 0 100%, 0 20px);
        }
        
        /* Tech pattern overlay */
        #progress_bar_container::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: 
                repeating-linear-gradient(90deg, 
                    transparent 0px,
                    transparent 20px,
                    rgba(0, 136, 204, 0.1) 20px,
                    rgba(0, 136, 204, 0.1) 21px),
                repeating-linear-gradient(0deg,
                    transparent 0px,
                    transparent 10px,
                    rgba(0, 136, 204, 0.05) 10px,
                    rgba(0, 136, 204, 0.05) 11px);
            animation: techGrid 5s linear infinite;
        }
        
        @keyframes techGrid {
            0% { transform: translate(0, 0); }
            100% { transform: translate(21px, 11px); }
        }
        
        /* Measurement marks */
        #progress_bar_container::after {
            content: '0% 10% 20% 30% 40% 50% 60% 70% 80% 90% 100%';
            position: absolute;
            bottom: -20px;
            left: 0;
            right: 0;
            color: #0088cc;
            font-family: 'Rajdhani', sans-serif;
            font-size: 10px;
            display: flex;
            justify-content: space-between;
            opacity: 0.5;
            letter-spacing: 1px;
        }

        #progress_bar_indicator {
            background: linear-gradient(90deg, 
                #0088cc 0%, 
                #00aaff 25%, 
                #00d4ff 50%, 
                #00aaff 75%, 
                #0088cc 100%);
            background-size: 200% 100%;
            height: 100%;
            width: 0%;
            transition: width 0.5s ease;
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
            animation: hudGlow 2s linear infinite;
            box-shadow: 
                0 0 30px rgba(0, 170, 255, 0.8),
                inset 0 0 20px rgba(0, 212, 255, 0.4);
            filter: brightness(1.2);
        }
        
        @keyframes hudGlow {
            0% { 
                background-position: 0% 50%;
                filter: brightness(1.2);
            }
            50% {
                filter: brightness(1.4);
            }
            100% { 
                background-position: 200% 50%;
                filter: brightness(1.2);
            }
        }
        
        /* Data flow effect */
        #progress_bar_indicator::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: linear-gradient(90deg, 
                transparent 0%, 
                rgba(255, 255, 255, 0.2) 50%, 
                transparent 100%);
            animation: dataFlow 1s linear infinite;
        }
        
        @keyframes dataFlow {
            0% { transform: translateX(-100%); }
            100% { transform: translateX(100%); }
        }

        #labelPercent {
            color: #000;
            background: linear-gradient(135deg, #00d4ff 0%, #00aaff 100%);
            padding: 6px 15px;
            font-family: 'Orbitron', monospace;
            font-weight: 900;
            font-size: 18px !important;
            font-size: 16px !important;
            z-index: 10;
            border: 1px solid #00d4ff;
            text-shadow: 0 1px 2px rgba(0, 0, 0, 0.5);
            clip-path: polygon(10px 0, 100% 0, 100% 100%, 0 100%, 0 10px);
            animation: percentData 0.5s infinite;
        }
        
        @keyframes percentData {
            0%, 90% { opacity: 1; }
            95% { opacity: 0.7; }
            100% { opacity: 1; }
        }

        .div_task_status {
            margin-top: 30px;
            padding: 25px;
            background: rgba(0, 11, 22, 0.8);
            border: 1px solid #0088cc;
            position: relative;
            backdrop-filter: blur(5px);
            clip-path: polygon(
                0 0, calc(100% - 20px) 0,
                100% 20px, 100% 100%,
                20px 100%, 0 calc(100% - 20px)
            );
        }
        
        /* HUD frame effect */
        .div_task_status::before {
            content: 'SYSTEM STATUS';
            position: absolute;
            top: -15px;
            left: 30px;
            background: #000;
            padding: 0 20px;
            color: #00aaff;
            font-family: 'Orbitron', monospace;
            font-size: 12px;
            font-weight: 700;
            letter-spacing: 3px;
            text-shadow: 0 0 10px rgba(0, 170, 255, 0.8);
        }
        
        /* Holographic shimmer */
        .div_task_status::after {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, 
                transparent 0%, 
                rgba(0, 170, 255, 0.1) 50%, 
                transparent 100%);
            animation: holoSweep 8s linear infinite;
            pointer-events: none;
        }
        
        @keyframes holoSweep {
            0% { left: -100%; }
            100% { left: 100%; }
        }

        .div_task_status table {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0 10px;
            font-family: 'Rajdhani', sans-serif;

            font-size: 11px !important;
            font-weight: 400;
            line-height: 1.4;
            color: #00aaff;
        }

        .div_task_status td {
            padding: 6px 23px;
            border: 1px solid rgba(0, 136, 204, 0.3);
            border-left: 3px solid #0088cc;
            vertical-align: middle;
            color: #00d4ff;
            background: linear-gradient(90deg, rgba(0, 136, 204, 0.1) 0%, transparent 50%);
            position: relative;
            transition: all 0.3s;
            font-size: 11px !important;
        }
        
        .div_task_status tr:hover td {
            background: linear-gradient(90deg, rgba(0, 136, 204, 0.2) 0%, rgba(0, 136, 204, 0.05) 100%);
            border-left-color: #00d4ff;
            text-shadow: 0 0 10px rgba(0, 212, 255, 0.6);
        }

        .div_task_status td:first-child {
            font-family: 'Orbitron', monospace;
            font-weight: 700;
            color: #0088cc;
            background: none;
            border: none;
            border-right: 1px solid rgba(0, 136, 204, 0.3);
            width: 200px;
            text-align: right;
            text-transform: uppercase;
            letter-spacing: 2px;
            font-size: 11px !important;
            padding-right: 30px;
            position: relative;
        }
        
        .div_task_status td:first-child::after {
            content: '//';
            position: absolute;
            right: 15px;
            color: #00aaff;
            opacity: 0.5;
        }

        .div_task_status td:nth-child(2) {
            padding-left: 30px;
            font-family: 'Orbitron', monospace;
            font-weight: 400;
            letter-spacing: 1px;
        }

        .status-complete {
            color: #00ff88 !important;
            text-shadow: 0 0 15px rgba(0, 255, 136, 0.8);
            font-weight: 700;
            letter-spacing: 2px;
            animation: completeGlow 2s ease-in-out infinite;
        }
        
        .status-complete::before {
            content: '[COMPLETE] ';
            color: #00ff88;
            font-size: 14px;
        }
        
        @keyframes completeGlow {
            0%, 100% { 
                text-shadow: 0 0 15px rgba(0, 255, 136, 0.8);
            }
            50% { 
                text-shadow: 0 0 25px rgba(0, 255, 136, 1), 0 0 35px rgba(0, 255, 136, 0.6);
            }
        }

        .status-running {
            color: #ffaa00 !important;
            text-shadow: 0 0 15px rgba(255, 170, 0, 0.8);
            font-weight: 700;
            letter-spacing: 2px;
            animation: processingData 0.5s infinite;
        }
        
        .status-running::before {
            content: '[PROCESSING] ';
            color: #ffaa00;
            font-size: 14px;
        }
        
        @keyframes processingData {
            0%, 100% { opacity: 1; }
            50% { opacity: 0.7; }
        }

        .status-error {
            color: #ff0044 !important;
            text-shadow: 0 0 15px rgba(255, 0, 68, 0.8);
            font-weight: 700;
            letter-spacing: 2px;
            animation: errorAlert 0.3s infinite;
        }
        
        .status-error::before {
            content: '[ERROR] ';
            color: #ff0044;
            font-size: 14px;
        }
        
        @keyframes errorAlert {
            0%, 100% { 
                transform: translateX(0);
            }
            25% { 
                transform: translateX(-2px);
            }
            75% { 
                transform: translateX(2px);
            }
        }
        
        /* Links in HUD theme */
        .div_task_status a, 
        .div_task_status a.mainbutton {
            color: #00d4ff;
            background: rgba(0, 136, 204, 0.1);
            border: 1px solid #0088cc;
            text-decoration: none;
            padding: 6px 20px;
            display: inline-block;
            font-family: 'Orbitron', monospace;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 2px;
            font-size: 12px !important;
            transition: all 0.3s;
            position: relative;
            clip-path: polygon(10px 0, 100% 0, 100% 100%, 0 100%, 0 10px);
        }
        
        .div_task_status a:hover,
        .div_task_status a.mainbutton:hover {
            background: rgba(0, 212, 255, 0.2);
            color: #fff;
            border-color: #00d4ff;
            text-decoration: none;
            box-shadow: 
                0 0 30px rgba(0, 212, 255, 0.8),
                inset 0 0 20px rgba(0, 212, 255, 0.2);
            text-shadow: 0 0 10px #00d4ff;
        }
        
        /* Additional HUD elements */
        span {
            color: #00d4ff;
        }
        
        /* Holographic glitch effect */
        @keyframes holoGlitch {
            0%, 100% { 
                text-shadow: 
                    0 0 10px rgba(0, 170, 255, 0.8),
                    2px 2px 0 rgba(255, 0, 170, 0.3),
                    -2px -2px 0 rgba(0, 255, 170, 0.3);
            }
            50% { 
                text-shadow: 
                    0 0 10px rgba(0, 170, 255, 0.8),
                    -2px 2px 0 rgba(255, 0, 170, 0.3),
                    2px -2px 0 rgba(0, 255, 170, 0.3);
            }
        }
        
        h1:hover {
            animation: holoGlitch 0.2s infinite;
        }
    `;
}