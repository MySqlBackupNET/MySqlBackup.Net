<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="DatabaseDumpViewer.aspx.cs" Inherits="System.pages.DatabaseDumpViewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        pre {
            background: #2f2f2f;
            border-radius: 15px;
        }
        code {
            border-radius: 15px;
        }
    </style>
    <link id="theme1" href="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.8.0/styles/a11y-dark.min.css" rel="stylesheet">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/highlight.min.js"></script>

    <style id="style2"></style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">
        <h1>Database Structure/Dump Viewer</h1>

        <asp:Button ID="btView" runat="server" Text="View Database's Tables Structure and Info" OnClick="btView_Click" />

        Theme:

        <select id="selectStyle" onchange="changeTheme()">
            <option>a11y-dark.min.css</option>
            <option>a11y-light.min.css</option>
            <option>agate.min.css</option>
            <option>an-old-hope.min.css</option>
            <option>androidstudio.min.css</option>
            <option>arduino-light.min.css</option>
            <option>arta.min.css</option>
            <option>ascetic.min.css</option>
            <option>atom-one-dark-reasonable.min.css</option>
            <option>atom-one-dark.min.css</option>
            <option>atom-one-light.min.css</option>
            <option>brown-paper.min.css</option>
            <option>codepen-embed.min.css</option>
            <option>color-brewer.min.css</option>
            <option>dark.min.css</option>
            <option>default.min.css</option>
            <option>devibeans.min.css</option>
            <option>docco.min.css</option>
            <option>far.min.css</option>
            <option>felipec.min.css</option>
            <option>foundation.min.css</option>
            <option>github-dark-dimmed.min.css</option>
            <option>github-dark.min.css</option>
            <option>github.min.css</option>
            <option>gml.min.css</option>
            <option>googlecode.min.css</option>
            <option>gradient-dark.min.css</option>
            <option>gradient-light.min.css</option>
            <option>grayscale.min.css</option>
            <option>hybrid.min.css</option>
            <option>idea.min.css</option>
            <option>intellij-light.min.css</option>
            <option>ir-black.min.css</option>
            <option>isbl-editor-dark.min.css</option>
            <option>isbl-editor-light.min.css</option>
            <option>kimbie-dark.min.css</option>
            <option>kimbie-light.min.css</option>
            <option>lightfair.min.css</option>
            <option>lioshi.min.css</option>
            <option>magula.min.css</option>
            <option>mono-blue.min.css</option>
            <option>monokai-sublime.min.css</option>
            <option>monokai.min.css</option>
            <option>night-owl.min.css</option>
            <option>nnfx-dark.min.css</option>
            <option>nnfx-light.min.css</option>
            <option>nord.min.css</option>
            <option>obsidian.min.css</option>
            <option>panda-syntax-dark.min.css</option>
            <option>panda-syntax-light.min.css</option>
            <option>paraiso-dark.min.css</option>
            <option>paraiso-light.min.css</option>
            <option>pojoaque.min.css</option>
            <option>purebasic.min.css</option>
            <option>qtcreator-dark.min.css</option>
            <option>qtcreator-light.min.css</option>
            <option>rainbow.min.css</option>
            <option>routeros.min.css</option>
            <option>school-book.min.css</option>
            <option>shades-of-purple.min.css</option>
            <option>srcery.min.css</option>
            <option selected>stackoverflow-dark.min.css</option>
            <option>stackoverflow-light.min.css</option>
            <option>sunburst.min.css</option>
            <option>tokyo-night-dark.min.css</option>
            <option>tokyo-night-light.min.css</option>
            <option>tomorrow-night-blue.min.css</option>
            <option>tomorrow-night-bright.min.css</option>
            <option>vs.min.css</option>
            <option>vs2015.min.css</option>
            <option>xcode.min.css</option>
            <option>xt256.min.css</option>
            <option>base16/3024.min.css</option>
            <option>base16/apathy.min.css</option>
            <option>base16/apprentice.min.css</option>
            <option>base16/ashes.min.css</option>
            <option>base16/atelier-cave-light.min.css</option>
            <option>base16/atelier-cave.min.css</option>
            <option>base16/atelier-dune-light.min.css</option>
            <option>base16/atelier-dune.min.css</option>
            <option>base16/atelier-estuary-light.min.css</option>
            <option>base16/atelier-estuary.min.css</option>
            <option>base16/atelier-forest-light.min.css</option>
            <option>base16/atelier-forest.min.css</option>
            <option>base16/atelier-heath-light.min.css</option>
            <option>base16/atelier-heath.min.css</option>
            <option>base16/atelier-lakeside-light.min.css</option>
            <option>base16/atelier-lakeside.min.css</option>
            <option>base16/atelier-plateau-light.min.css</option>
            <option>base16/atelier-plateau.min.css</option>
            <option>base16/atelier-savanna-light.min.css</option>
            <option>base16/atelier-savanna.min.css</option>
            <option>base16/atelier-seaside-light.min.css</option>
            <option>base16/atelier-seaside.min.css</option>
            <option>base16/atelier-sulphurpool-light.min.css</option>
            <option>base16/atelier-sulphurpool.min.css</option>
            <option>base16/atlas.min.css</option>
            <option>base16/bespin.min.css</option>
            <option>base16/black-metal-bathory.min.css</option>
            <option>base16/black-metal-burzum.min.css</option>
            <option>base16/black-metal-dark-funeral.min.css</option>
            <option>base16/black-metal-gorgoroth.min.css</option>
            <option>base16/black-metal-immortal.min.css</option>
            <option>base16/black-metal-khold.min.css</option>
            <option>base16/black-metal-marduk.min.css</option>
            <option>base16/black-metal-mayhem.min.css</option>
            <option>base16/black-metal-nile.min.css</option>
            <option>base16/black-metal-venom.min.css</option>
            <option>base16/black-metal.min.css</option>
            <option>base16/brewer.min.css</option>
            <option>base16/bright.min.css</option>
            <option>base16/brogrammer.min.css</option>
            <option>base16/brush-trees-dark.min.css</option>
            <option>base16/brush-trees.min.css</option>
            <option>base16/chalk.min.css</option>
            <option>base16/circus.min.css</option>
            <option>base16/classic-dark.min.css</option>
            <option>base16/classic-light.min.css</option>
            <option>base16/codeschool.min.css</option>
            <option>base16/colors.min.css</option>
            <option>base16/cupcake.min.css</option>
            <option>base16/cupertino.min.css</option>
            <option>base16/danqing.min.css</option>
            <option>base16/darcula.min.css</option>
            <option>base16/dark-violet.min.css</option>
            <option>base16/darkmoss.min.css</option>
            <option>base16/darktooth.min.css</option>
            <option>base16/decaf.min.css</option>
            <option>base16/default-dark.min.css</option>
            <option>base16/default-light.min.css</option>
            <option>base16/dirtysea.min.css</option>
            <option>base16/dracula.min.css</option>
            <option>base16/edge-dark.min.css</option>
            <option>base16/edge-light.min.css</option>
            <option>base16/eighties.min.css</option>
            <option>base16/embers.min.css</option>
            <option>base16/equilibrium-dark.min.css</option>
            <option>base16/equilibrium-gray-dark.min.css</option>
            <option>base16/equilibrium-gray-light.min.css</option>
            <option>base16/equilibrium-light.min.css</option>
            <option>base16/espresso.min.css</option>
            <option>base16/eva-dim.min.css</option>
            <option>base16/eva.min.css</option>
            <option>base16/flat.min.css</option>
            <option>base16/framer.min.css</option>
            <option>base16/fruit-soda.min.css</option>
            <option>base16/gigavolt.min.css</option>
            <option>base16/github.min.css</option>
            <option>base16/google-dark.min.css</option>
            <option>base16/google-light.min.css</option>
            <option>base16/grayscale-dark.min.css</option>
            <option>base16/grayscale-light.min.css</option>
            <option>base16/green-screen.min.css</option>
            <option>base16/gruvbox-dark-hard.min.css</option>
            <option>base16/gruvbox-dark-medium.min.css</option>
            <option>base16/gruvbox-dark-pale.min.css</option>
            <option>base16/gruvbox-dark-soft.min.css</option>
            <option>base16/gruvbox-light-hard.min.css</option>
            <option>base16/gruvbox-light-medium.min.css</option>
            <option>base16/gruvbox-light-soft.min.css</option>
            <option>base16/hardcore.min.css</option>
            <option>base16/harmonic16-dark.min.css</option>
            <option>base16/harmonic16-light.min.css</option>
            <option>base16/heetch-dark.min.css</option>
            <option>base16/heetch-light.min.css</option>
            <option>base16/helios.min.css</option>
            <option>base16/hopscotch.min.css</option>
            <option>base16/horizon-dark.min.css</option>
            <option>base16/horizon-light.min.css</option>
            <option>base16/humanoid-dark.min.css</option>
            <option>base16/humanoid-light.min.css</option>
            <option>base16/ia-dark.min.css</option>
            <option>base16/ia-light.min.css</option>
            <option>base16/icy-dark.min.css</option>
            <option>base16/ir-black.min.css</option>
            <option>base16/isotope.min.css</option>
            <option>base16/kimber.min.css</option>
            <option>base16/london-tube.min.css</option>
            <option>base16/macintosh.min.css</option>
            <option>base16/marrakesh.min.css</option>
            <option>base16/materia.min.css</option>
            <option>base16/material-darker.min.css</option>
            <option>base16/material-lighter.min.css</option>
            <option>base16/material-palenight.min.css</option>
            <option>base16/material-vivid.min.css</option>
            <option>base16/material.min.css</option>
            <option>base16/mellow-purple.min.css</option>
            <option>base16/mexico-light.min.css</option>
            <option>base16/mocha.min.css</option>
            <option>base16/monokai.min.css</option>
            <option>base16/nebula.min.css</option>
            <option>base16/nord.min.css</option>
            <option>base16/nova.min.css</option>
            <option>base16/ocean.min.css</option>
            <option>base16/oceanicnext.min.css</option>
            <option>base16/one-light.min.css</option>
            <option>base16/onedark.min.css</option>
            <option>base16/outrun-dark.min.css</option>
            <option>base16/papercolor-dark.min.css</option>
            <option>base16/papercolor-light.min.css</option>
            <option>base16/paraiso.min.css</option>
            <option>base16/pasque.min.css</option>
            <option>base16/phd.min.css</option>
            <option>base16/pico.min.css</option>
            <option>base16/pop.min.css</option>
            <option>base16/porple.min.css</option>
            <option>base16/qualia.min.css</option>
            <option>base16/railscasts.min.css</option>
            <option>base16/rebecca.min.css</option>
            <option>base16/ros-pine-dawn.min.css</option>
            <option>base16/ros-pine-moon.min.css</option>
            <option>base16/ros-pine.min.css</option>
            <option>base16/sagelight.min.css</option>
            <option>base16/sandcastle.min.css</option>
            <option>base16/seti-ui.min.css</option>
            <option>base16/shapeshifter.min.css</option>
            <option>base16/silk-dark.min.css</option>
            <option>base16/silk-light.min.css</option>
            <option>base16/snazzy.min.css</option>
            <option>base16/solar-flare-light.min.css</option>
            <option>base16/solar-flare.min.css</option>
            <option>base16/solarized-dark.min.css</option>
            <option>base16/solarized-light.min.css</option>
            <option>base16/spacemacs.min.css</option>
            <option>base16/summercamp.min.css</option>
            <option>base16/summerfruit-dark.min.css</option>
            <option>base16/summerfruit-light.min.css</option>
            <option>base16/synth-midnight-terminal-dark.min.css</option>
            <option>base16/synth-midnight-terminal-light.min.css</option>
            <option>base16/tango.min.css</option>
            <option>base16/tender.min.css</option>
            <option>base16/tomorrow-night.min.css</option>
            <option>base16/tomorrow.min.css</option>
            <option>base16/twilight.min.css</option>
            <option>base16/unikitty-dark.min.css</option>
            <option>base16/unikitty-light.min.css</option>
            <option>base16/vulcan.min.css</option>
            <option>base16/windows-10-light.min.css</option>
            <option>base16/windows-10.min.css</option>
            <option>base16/windows-95-light.min.css</option>
            <option>base16/windows-95.min.css</option>
            <option>base16/windows-high-contrast-light.min.css</option>
            <option>base16/windows-high-contrast.min.css</option>
            <option>base16/windows-nt-light.min.css</option>
            <option>base16/windows-nt.min.css</option>
            <option>base16/woodland.min.css</option>
            <option>base16/xcode-dusk.min.css</option>
            <option>base16/zenburn.min.css</option>
        </select>

        Font:

        <select id="selectFont">
            <option>Roboto Mono</option>
            <option>Consolas</option>
            <option>Cascadia Mono</option>
            <option>Inconsolata</option>
            <option>Source Code Pro</option>
            <option>IBM Plex Mono</option>
            <option>Space Mono</option>
            <option>PT Mono</option>
            <option>Ubuntu Mono</option>
            <option>Nanum Gothic Coding</option>
            <option>Cousine</option>
            <option>Fira Mono</option>
            <option>Share Tech Mono</option>
            <option>Courier Prime</option>
            <option>Anonymous Pro</option>
            <option>Cutive Mono</option>
            <option>VT323</option>
            <option selected>JetBrains Mono</option>
            <option>Noto Sans Mono</option>
            <option>Red Hat Mono</option>
            <option>Martian Mono</option>
            <option>Major Mono Display</option>
            <option>Nova Mono</option>
            <option>Syne Mono</option>
            <option>Xanh Mono</option>
            <option>Monofett</option>
        </select>

        <hr />
        <asp:CheckBox ID="cbExportRows" runat="server" />
        Export Rows (only show rows if you have small amount of rows.)
        <input id="inputFontSize" type="number" step=".1" style="width: 50px;" value="11" /> Font Size (PT)
        <input id="inputLineHeight" type="number" style="width: 60px;" value="150" /> Line Height (%)
        <hr />

        <pre><code class="language-sql"><asp:Literal ID="ltOutput" runat="server"></asp:Literal></code></pre>

        <script>
            // Load preferences from localStorage on page load
            window.addEventListener('DOMContentLoaded', function () {
                loadPreferences();
                // Use timeout to ensure content is loaded before highlighting
                setTimeout(() => { hljs.highlightAll(); }, 100);

                // Add event listeners after DOM is loaded
                document.getElementById('selectFont').addEventListener('change', function () {
                    updateFont();
                    savePreferences();
                });

                document.getElementById('inputFontSize').addEventListener('input', function () {
                    updateFont();
                    savePreferences();
                });

                document.getElementById('inputLineHeight').addEventListener('input', function () {
                    updateFont();
                    savePreferences();
                });
            });

            // Load saved preferences
            function loadPreferences() {
                const savedPrefs = localStorage.getItem('DatabaseDumpViewerThemeFont');
                if (savedPrefs) {
                    const prefs = JSON.parse(savedPrefs);

                    // Apply saved theme
                    if (prefs.theme) {
                        document.getElementById('selectStyle').value = prefs.theme;
                        document.getElementById('theme1').href = `https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.8.0/styles/${prefs.theme}`;
                    }

                    // Apply saved font
                    if (prefs.font) {
                        document.getElementById('selectFont').value = prefs.font;
                    }

                    // Apply saved font size
                    if (prefs.fontSize) {
                        document.getElementById('inputFontSize').value = prefs.fontSize;
                    }

                    // Apply saved line height
                    if (prefs.lineHeight) {
                        document.getElementById('inputLineHeight').value = prefs.lineHeight;
                    }

                    // Update font after setting values
                    updateFont();
                }
            }

            // Save preferences to localStorage
            function savePreferences() {
                const prefs = {
                    theme: document.getElementById('selectStyle').value,
                    font: document.getElementById('selectFont').value,
                    fontSize: document.getElementById('inputFontSize').value,
                    lineHeight: document.getElementById('inputLineHeight').value
                };
                localStorage.setItem('DatabaseDumpViewerThemeFont', JSON.stringify(prefs));
            }

            // Change theme and save preference
            function changeTheme() {
                const selectStyle = document.getElementById('selectStyle');
                document.getElementById('theme1').href = `https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.8.0/styles/${selectStyle.value}`;
                savePreferences();
            }

            // Update font function
            function updateFont() {
                let selectFont = document.getElementById('selectFont');
                let fontName = selectFont.options[selectFont.selectedIndex].text;
                let fontNameUrl = fontName.replace(' ', '+');
                let inputFontSize = document.getElementById('inputFontSize');
                let inputLineHeight = document.getElementById('inputLineHeight');

                document.getElementById('style2').textContent = `
            @import url('https://fonts.googleapis.com/css2?&display=swap&family=${fontNameUrl}');
            pre, pre code, pre code span {
                font-family: "${fontName}", monospace !important;
                font-size: ${inputFontSize.value}pt !important;
                line-height: ${inputLineHeight.value}% !important;
            }`;
            }
        </script>

    </div>

</asp:Content>
