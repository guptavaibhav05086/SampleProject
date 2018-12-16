var app = angular.module("myapp", ["ngRoute"]);

//app.config(function ($routeProvider) {
//    $routeProvider  
//    .when("/", {
//        templateUrl: "ProvisionNewProject.htm"
//        //controller: "londonCtrl"
//    })
//    .when("/ProjectView", {
//        templateUrl: "ProjectView.htm"
//        //controller: "/HTMLTemplates/ProjectView.htm"
//    })
//    //.when("/Reports", {
//    //    templateUrl: "main.htm",
//    //    controller: "londonCtrl"
//    //})
    
//});

app.config(function ($routeProvider) {
    $routeProvider
    .when("/", {
        templateUrl: "Index.html"
    })
    .when("/london", {
        templateUrl: "ProjectView.htm"
    })
    .when("/paris", {
        templateUrl: "ProjectView.htm"
    });
});
