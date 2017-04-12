app.config(['$locationProvider', '$stateProvider', '$urlRouterProvider', 'PATH', function ($locationProvider, $stateProvider, $urlRouterProvider, PATH) {

    $locationProvider.html5Mode(true);
    $urlRouterProvider.otherwise('/');

    $stateProvider

     // HOME STATES AND NESTED VIEWS ========================================
     .state('Category', {
         url: '/Category',
         templateUrl: PATH + 'Category_Partial.html',

     })
    .state('Forum', {
        url: '/Forum',
        templateUrl: PATH + 'Forum_Partial.html'
    })
}]);