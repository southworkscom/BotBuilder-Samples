var builder = require('botbuilder');

const Today = 'Today';
const Tomorrow = 'Tomorrow';

const lib = new builder.Library('delivery');
lib.dialog('date', [
    function (session, args, next) {
        builder.Prompts.choice(session, 'When would you like these delivered?', [Today, Tomorrow]);
    },
    function (session, args) {
        var deliveryDate = args.response.entity === Today ? new Date() : new Date().addDays(1);
        session.endDialogWithResult({
            deliveryDate: deliveryDate
        });
    }
]);

// Helpers
Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
};

// Export createLibrary() function
module.exports.createLibrary = function () {
    return lib.clone();
};