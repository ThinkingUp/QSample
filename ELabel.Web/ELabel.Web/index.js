var app = angular.module('eLabel', ['ngRoute']);

app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
       
         when('/analysis', {
             templateUrl: 'Templates/Analysis/analysis.html',
             controller: 'AnalysisController'
         }).
        when('/alerts', {
            templateUrl: 'Templates/Alerts/alerts.html',
            controller: 'AlertsController'
        }).
        when('/samples', {
            templateUrl: 'Templates/Samples/samples.html',
            controller: 'SamplesController'
        }).
        when('/home', {
            templateUrl: 'Templates/Home/home.html',
            controller: 'HomeController'
        }).
        when('/message', {
            templateUrl: 'Templates/Message/message.html',
            controller: 'MessageController'
        });
  }]);

app.controller('TabController', function () {
    this.tab = 1;

    this.selectTab = function (setTab) {
        this.tab = setTab;
    };
    this.isSelected = function (checkTab) {
        return this.tab === checkTab;
    };
});


app.controller('MainCtrl', ['$scope', '$http',
  function ($scope, $http) {

      $scope.date = new Date();
      $scope.method = 'GET';
      $scope.labUrl = '../api/Laboratory_analysis?pageIndex=1&pageSize=10';
      $scope.drillUrl = '../api/DrillholeSamplesViews?pageIndex=1&pageSize=10';
      $scope.alertUrl = '../api/AlertGenerators?pageIndex=0&pageSize=1';
      $scope.recoveryUrl = '../api/RecoveryIntervals?pageIndex=0&pageSize=5';
      $scope.rejectUrl = '../api/RejectAlerts/latest?pageIndex=0&pageSize=5';
      $scope.rejectAllUrl = '../api/RejectAlerts?pageIndex=0&pageSize=5';
      $scope.statusUrl = '../api/Labels/status';
      $scope.labResults = [];
      $scope.drillResults = [];
      $scope.alertResultS = [];
      $scope.recoveryResults = [];
      $scope.rejectResults = [];
      $scope.rejectAllResults = [];
      $scope.statusResults = [];
      $http.get($scope.labUrl)
     .success(function (response) {
         $scope.labResults = response;
       
     });

      $http.get($scope.drillUrl)
     .success(function (response) {
         $scope.drillResults = response;
         
      });
      
      $http.get($scope.alertUrl)
        .success(function (response) {
           
            $scope.alertResultS = angular.fromJson(response[0].Message);
            
        });

      $http.get($scope.recoveryUrl)
        .success(function (response) {
            
            $scope.recoveryResults = angular.fromJson(response);
            
        });
      $http.get($scope.rejectUrl)
       .success(function (response) {
            console.log(response);
           $scope.rejectResults = angular.fromJson(response);
           console.log($scope.rejectResults);
       });

      $http.get($scope.rejectAllUrl)
        .success(function (response) {   
           $scope.rejectAllResults = angular.fromJson(response);  
        });

      $http.get($scope.statusUrl)
       .success(function (response) {
          console.log(response);
          $scope.statusResults = angular.fromJson(response);
          console.log($scope.statusResults);
      });
  }]);
  