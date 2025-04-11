import { getCurrentTime } from "./time_lib";
import { generarPDF } from "./jsPdf_lib";
import { generarContrasena } from "./contraseñaGen";
import { redirectBlank } from "./Redirect_Blank";
import { writeCookie, readCookie } from "./cookies";
import { wasmReload } from "./utilidades";
import { generarPDFCheques } from "./jsPdf_lib";

//prueba getCurrentTime
export function GetCurrentTime() {
  return getCurrentTime();
}

//Para generar PDF, se Puede agregar mas funciones en src jsPdf_lib
export function GenerarPDF(
  nombreDelArchivo,
  datosCliente,
  Elements,
  cabecera,
  selectedDireccionFact
) {
  return generarPDF(
    nombreDelArchivo,
    datosCliente,
    Elements,
    cabecera,
    selectedDireccionFact
  );
}

export function GenerarPDFCheques(Cheques) {
  return generarPDFCheques(Cheques);
}

//genera una contraseña segun el largo que se le de sin simbolos
export function GenerarContrasena(longitud) {
  return generarContrasena(longitud);
}

//Redirige a una pafina en blanco segun el url que se le de
export function RedirectBlank(url) {
  return redirectBlank(url);
}

//para generar cookies(se le manda el nombre del valor, el valor y el tiempo de duracion) y leerlas
//Todo: Migrar a Typescript para generar interfaces y generar mayor seguridad
export function WriteCookie(name, value, days) {
  return writeCookie(name, value, days);
}

export function ReadCookie(cname) {
  return readCookie(cname);
}

export function WasmReload() {
  return wasmReload();
}
