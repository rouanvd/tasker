'use strict';

////////////////
// Properties //
////////////////


exports.permissionImpl = function() {
      return Notification.permission; 
};

exports.mkNotificationImpl = function(s, options) {
    var myNotification = new Notification(s, options);
    return myNotification;
};

/////////////
// Methods //
/////////////

exports.requestPermissionImpl = function() {
  return Notification.requestPermission();
};

exports.closeImpl = function(n, time) {
    return function() {
      setTimeout(n.close.bind(n), time);
      return {};
    };
};

////////////
// Events //
////////////
