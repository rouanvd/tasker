'use strict';

exports.typeError_message_ffi = function(typeErr) {
  var msg = typeErr.message;
  if (msg == null)
    return "";

  return msg;
};


exports.typeError_name_ffi = function(typeErr) {
  var name = typeErr.name;
  if (name == null)
    return "";

  return name;
};

