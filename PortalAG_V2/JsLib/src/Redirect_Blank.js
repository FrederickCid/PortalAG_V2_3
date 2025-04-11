export const redirectBlank = (url) => {
  const newWindow = window.open(url, "_Blank", "fullscreen=yes");
  if (newWindow) {
    newWindow.focus();
    //newWindow.print();
  }
};
