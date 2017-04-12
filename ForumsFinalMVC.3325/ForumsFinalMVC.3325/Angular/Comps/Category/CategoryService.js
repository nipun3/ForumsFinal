app.service('categoryService', categoryService);
categoryService.$inject = ['$http', '$q'];

function categoryService($http, $q) {

    var urlBase = 'http://localhost:53758/api/category';

    this.getcategories = function () {
        return $http.get(urlBase);
    };
    this.getcategory = function (id) {
        return $http.get(urlBase + '/' + id);
    };

    this.insertcategory = function (cust) {
        return $http.post(urlBase, cust);
    };

    this.updatecategory = function (cust) {
        return $http.put(urlBase + '/' + cust.ID, cust)
    };

    this.deletecategory = function (id) {
        return $http.delete(urlBase + '/' + id);
    };

    this.showAddCategoryForm = function (elementId) {
        $("." + elementId + "").show();
        $(".edit-content").slideUp("slow");
        $(".add-content").slideDown("slow");
        this.focusToElement(elementId);
    };
    this.showEditCategoryForm = function (elementId) {
        $("." + elementId + "").show();
        $(".add-content").slideUp("slow");
        $(".edit-content").slideDown("slow");
        this.focusToElement(elementId);
    };
    this.focusToElement = function (elementId) {
        $('html, body').animate({ scrollTop: $('#' + elementId).position().top }, 'slow');
    };
    this.focusToTop = function () {
        $('html, body').animate({ scrollTop: 0 }, 800);
    };
    this.resetPage = function () {
        $(".add-content").slideUp("slow");
        $(".edit-content").slideUp("slow");
        $(".alert").hide();
        $(".modal-header.alert.alert-success").show();
        $("." + elementId + "").hide();
        this.focusToTop();
    };

}