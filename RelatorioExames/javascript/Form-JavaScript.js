function CheckDate(pObj) {
    var expReg = /^((0[1-9]|[12]\d)\/(0[1-9]|1[0-2])|30\/(0[13-9]|1[0-2])|31\/(0[13578]|1[02]))\/(19|20)?\d{2}$/;
    var aRet = true;
    if ((pObj) && (pObj.value.match(expReg)) && (pObj.value != '')) {
        var dia = pObj.value.substring(0, 2);
        var mes = pObj.value.substring(3, 5);
        var ano = pObj.value.substring(6, 10);
        if ((mes == 4 || mes == 6 || mes == 9 || mes == 11) && dia > 30)
            aRet = false;
        else
            if ((ano % 4) != 0 && mes == 2 && dia > 28)
                aRet = false;
            else
                if ((year % 400 == 0 || (year % 100 != 0 && ano % 4 == 0)) && mes == 2 && dia > 29)
                    aRet = false;
    } else
        aRet = false;

    if (!aRet) {
        alert('Atenção! Data inexistente, por favor edite novamente.');
        pObj.value = "";

    }
    return aRet;
}
function CheckDate2(pObj) {
    var expReg = /^((0[1-9]|[12]\d)\/(0[1-9]|1[0-2])|30\/(0[13-9]|1[0-2])|31\/(0[13578]|1[02]))\/(19|20)?\d{2}\s([0-1]\d|[2][0-3]):[0-5][0-9]$/;
    var aRet = true;
    if ((pObj) && (pObj.value.match(expReg)) && (pObj.value != '')) {
        var dia = pObj.value.substring(0, 2);
        var mes = pObj.value.substring(3, 5);
        var ano = pObj.value.substring(6, 10);
        if ((mes == 4 || mes == 6 || mes == 9 || mes == 11) && dia > 30)
            aRet = false;
        else
            if ((ano % 4) != 0 && mes == 2 && dia > 28)
                aRet = false;
            else
                if ((year % 400 == 0 || (year % 100 != 0 && ano % 4 == 0)) && mes == 2 && dia > 29)
                    aRet = false;
    } else
        aRet = false;

    if (!aRet) {
        alert('Atenção! Data ou hora inexistente, por favor edite novamente.');
        pObj.value = "";

    }
    return aRet;
}
$(document).ready(function () {
 

    $('.input-dsi').mask('99/99/9999');
    $('.input-dsf').mask('99/99/9999');

    $('.input-dai').mask('99/99/9999 99:99');
    $('.input-daf').mask('99/99/9999 99:99');



});

