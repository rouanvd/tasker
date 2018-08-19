'use strict';

exports.readProp = function (prop, obj) {
  return obj[prop];
};


exports.setPropImpl = function (unit, prop, value, obj) {
  return function() { // Effect wrapper
    obj[prop] = value;
    return unit;
  };
};


exports.setPropOrNullImpl = function (isJustF, fromJustF, unit, prop, maybeVal, obj) {
  return function() { // Effect wrapper
    if (isJustF( maybeVal ))
      obj[prop] = fromJustF( maybeVal );
    else
      obj[prop] = null;

    return unit;
  };
};


exports.setPropEffFuncImpl = function (unit, prop, effFunc, obj) {
  return function() { // Effect wrapper
    obj[prop] = function() { effFunc(unit)(/*Eff*/); };
    return unit;
  };
};


exports.setPropEventHandlerImpl = function (unit, prop, eventHandlerFunc, obj) {
  return function() { // Effect wrapper
    obj[prop] = function(e) { eventHandlerFunc( e )(/*Eff*/); };
    return unit;
  };
};


exports.runEffMethod0Impl = function (unitVal, method, obj) {
  return function () { // Effect wrapper
    obj[method]();
    return unitVal;
  };
};
