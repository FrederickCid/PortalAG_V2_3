import moment from 'moment';

export function getCurrentTime() {
    return `La fecha actual es: ${moment().format("MMM Do YY")}`
}