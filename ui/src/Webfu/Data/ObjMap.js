'use strict';

exports._copyEff = function (m) {
  return function () {
    var r = {};
    for (var k in m) {
      if (hasOwnProperty.call(m, k)) {
        r[k] = m[k];
      }
    }
    return r;
  };
};

exports.empty = {};

exports.allKeys = function (predF, obj) {
  for (var key in obj) {
    if (hasOwnProperty.call(obj, key) && !predF(key))
      return false;
  }

  return true;
};

exports._lookupKey = function (no, yesF, key, obj) {
  return key in obj ? yesF(key) : no;
};


exports._insert = function (k, v, obj) {
  var objCopy = exports._copyEff( obj )();
  objCopy[k] = v;
  return objCopy;
};
