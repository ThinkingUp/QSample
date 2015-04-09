app.controller('AlertsController', function ($scope) {
    $scope.title = "Alert";


    $scope.templates = [{ title: "Alerts Definition", url: "Templates/Alerts/Alerts_Definition/alerts_definition.html" },
                         { title: "Recent Alerts", url: "Templates/Alerts/Recent_Alerts/recent_alerts.html" },
                         { title: "Add Alerts", url: "Templates/Alerts/Add_Alerts/add_alerts.html" }

    ]; 
    $scope.template_alertsDefinition = $scope.templates[0];
    $scope.template_recentAlerts = $scope.templates[1];
    $scope.template_addAlerts = $scope.templates[2];
    $scope.init = function($scope, $http) {
       
            $scope.method = 'GET';        
            $scope.alertUrl = '../api/AlertGenerators?alertTableName=ALERTSSTANDARDREFERENCE';
            $scope.alertResultS = [];
            $http.get($scope.alertUrl)
              .success(function (response) {
                  console.log(response);
                  $scope.alertResultS = angular.fromJson(response[0].Message);
                  console.log($scope.alertResultS);
              }); 
    };
});