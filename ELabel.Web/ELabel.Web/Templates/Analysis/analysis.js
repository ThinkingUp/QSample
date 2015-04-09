app.controller('AnalysisController', function ($scope) {
    $scope.title = "Analysis";


    $scope.templates = [{ title: "Recovery Interval", url: "Templates/Analysis/Recovery_Interval/recovery_interval.html" }];                           
    $scope.template_recoveryInterval= $scope.templates[0];
    $scope.init = function () { };
});