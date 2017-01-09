var builder = require('botbuilder');
var locationDialog = require('botbuilder-location');

const lib = new builder.Library('address');

// Address constants
const InvalidAddress = 'Sorry, I could not understand that address. Can you try again? (Number, street, city, state, and ZIP)';
const ConfirmChoice = 'Use this address';
const EditChoice = 'Edit';

lib.library(locationDialog.createLibrary(process.env.BING_MAPS_KEY));

lib.dialog('/', [
    function (session, args) {
        // Ask for address
        args = args || {};
        var promptMessage = args.promptMessage || 'Address?';
        session.dialogData.promptMessage = promptMessage;

        // Use botbuilder-location dialog for address request
        var options = {
            prompt: promptMessage,
            useNativeControl: true,
            reverseGeocode: true,
            requiredFields:
                locationDialog.LocationRequiredFields.streetAddress |
                locationDialog.LocationRequiredFields.locality |
                locationDialog.LocationRequiredFields.country
        };

        locationDialog.getLocation(session, options);
    },
    function (session, results) {
        if (results.response) {
            // Return selected address to previous dialog in stack
            var place = results.response;
            var address = locationDialog.getFormattedAddressFromPlace(place, ", ");
            session.endDialogWithResult({
                address: address
            });
        } else {
            // No address resolved, restart
            session.replaceDialog('/', { promptMessage: session.dialogData.promptMessage });
        }
}]);

// Request Billing Address
// Prompt/Save selected address. Uses previous dialog to request and validate address. 
const UseSavedInfoChoices = {
    Home: 'Home address',
    Work: 'Work address',
    NotThisTime: 'No, thanks!'
};
lib.dialog('billing', [
    function (session, args, next) {
        var selection = session.message.text;
        var saved = session.userData.billingAddresses = session.userData.billingAddresses || {};
        if (hasAddresses(saved)) {
            // Saved data found, check for selection
            if (selection && saved[selection]) {
                // Retrieve selection
                var savedAddress = saved[selection];
                session.dialogData.billingAddress = savedAddress;
                next();
            } else if (selection === UseSavedInfoChoices.NotThisTime) {
                // Ask for data
                next();
            } else {
                // No selection, prompt which saved address to use
                session.send('Please select your billing address');

                var message = new builder.Message(session)
                    .attachmentLayout(builder.AttachmentLayout.carousel);
                var homeAddress = saved[UseSavedInfoChoices.Home];
                var workAddress = saved[UseSavedInfoChoices.Work];
                if (homeAddress) message.addAttachment(createAddressCard(session, UseSavedInfoChoices.Home, homeAddress));
                if (workAddress) message.addAttachment(createAddressCard(session, UseSavedInfoChoices.Work, workAddress));
                message.addAttachment(createAddressCard(session, UseSavedInfoChoices.NotThisTime, 'Add a new address'));
                session.send(message);
            }
        } else {
            // No data
            next();
        }
    },
    function (session, args, next) {
        if (session.dialogData.billingAddress) {
            // Address selected in previous step, skip
            return next();
        }

        // Ask for address
        session.beginDialog('/',
            {
                promptMessage: 'What\'s your billing address? Include apartment # if needed.'
            });
    },
    function (session, args, next) {
        if (session.dialogData.billingAddress) {
            return next();
        }

        // Retrieve address from previous dialog
        session.dialogData.billingAddress = args.address;

        // Ask to save address
        var options = [UseSavedInfoChoices.Home, UseSavedInfoChoices.Work, UseSavedInfoChoices.NotThisTime];
        builder.Prompts.choice(session, 'Would you like to save this address?', options);
    },
    function (session, args, next) {
        var billingAddress = session.dialogData.billingAddress;

        if (args.response && args.response.entity !== UseSavedInfoChoices.NotThisTime) {
            // Save address
            session.userData.billingAddresses = session.userData.billingAddresses || {};
            session.userData.billingAddresses[args.response.entity] = billingAddress;
        }

        // Return address 
        session.endDialogWithResult({ billingAddress: billingAddress });
    }
]);


// Helpers
function hasAddresses(addresses) {
    return !!addresses[UseSavedInfoChoices.Home] || !!addresses[UseSavedInfoChoices.Work];
}

function createAddressCard(session, buttonTitle, address) {
    return new builder.HeroCard(session)
        .title(buttonTitle)
        .subtitle(address)
        .buttons([
            builder.CardAction.imBack(session, buttonTitle, buttonTitle)
        ]);
}

module.exports.UseSavedInfoChoices = UseSavedInfoChoices;

// Export createLibrary() function
module.exports.createLibrary = function () {
    return lib.clone();
};