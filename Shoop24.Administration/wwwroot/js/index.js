function GetDarkMode(){
    return localStorage.getItem("isDarkMode") === "true";
}

function SetDarkMode(isDarkMode){
    ToggleDarkMode();
    
    localStorage.setItem("isDarkMode", isDarkMode);
}

function ToggleDarkMode() {
    document.body.classList.toggle("fro-dark-context");
    document.body.classList.toggle("fro-dark-background");

    var html = document.getElementsByTagName("html")[0];
    html.classList.toggle("fro-dark-context");

    var isDarkMode = document.body.classList.contains("fro-dark-context");
    window.localStorage.setItem("isDarkMode", isDarkMode);
}