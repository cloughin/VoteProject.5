// Initialize when ready
$(function () {

  var $dropdown = $('.email-form-subject');
  
  function onChange() {
    $dropdown.val() === "Other" ? $(".email-form-other-subject").show(200) : $(".email-form-other-subject").hide(200);
  }

  $dropdown.change(onChange);
  onChange();
  var errorLabel = $('.email-form-error-label');
  var goodLabel = $('.email-form-good-label');
  if (errorLabel.length !== 0)
    UTIL.alert(errorLabel.text());
  else if (goodLabel.length !== 0)
    UTIL.alert(goodLabel.text());
});
