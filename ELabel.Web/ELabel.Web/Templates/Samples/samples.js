app.controller('SamplesController', function ($scope) {
    $scope.title = "Samples";


    $scope.templates = [{ title: "Label", url: "Templates/Samples/Label/label.html" },
                         { title: "Label History", url: "Templates/Samples/Label_History/label_history.html" }

    ];

    $scope.template_label = $scope.templates[0];
    $scope.template_labelHistory = $scope.templates[1];
});