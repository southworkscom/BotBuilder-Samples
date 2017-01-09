var builder = require('botbuilder');

const lib = new builder.Library('help');
lib.dialog('/', builder.DialogAction.endDialog('Support will contact you shortly. Have a nice day :)'));

// Export createLibrary() function
module.exports.createLibrary = function () {
    return lib.clone();
};