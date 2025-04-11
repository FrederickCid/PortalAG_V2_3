export function generarContrasena(longitud) {
    var caracteres = {
        numeros: '0123456789',
        //simbolos: '!@#$%^&*()_-+={[}]<>.,?/',
        mayusculas: 'ABCDEFGHIJKLMNOPQRSTUVWXYZ',
        minusculas: 'abcdefghijklmnopqrstuvwxyz'
    };

    var caracteresTotales = Object.values(caracteres).join('');
    var contrasena = '';

    for (var i = 0; i < longitud; i++) {
        var indiceAleatorio = Math.floor(Math.random() * caracteresTotales.length);
        contrasena += caracteresTotales.charAt(indiceAleatorio);
    }

    return(contrasena);
}