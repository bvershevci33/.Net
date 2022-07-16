// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


generateUniqueCode = (length) => {
    var result = '';
    var letters = generateLetters();
    var numbers = generateNumbers();

    var char = letters + numbers;
    var charLength = char.length;

    for (var i = 0; i < length; i++) {
        result += char.charAt(Math.floor(Math.random() * charLength));
    }
    return result;
};

generateLetters = () => {
    var letters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';

    return letters;
};

generateNumbers = () => {
    var numbers = '0123456789';

    return numbers;
};