var require = {
    deps: [],
    locale: 'en',
    baseUrl: '/Scripts',
    paths: {
        // 3rd party libs
        // jquery
        "jquery": "jquery-1.9.1",

        // amplify
        "amplify-lib": "amplify",
        "amplify": "App/amplify-setup"
    },
    shim: {
        //"mockjax": { deps: ["jquery"] },
        //"underscore": { deps: [], exports: '_' }
        "amplify-lib": { deps: ["jquery"], exports: 'amplify' }
    }
};