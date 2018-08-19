'use strict';


exports.mkPromiseImpl = function(executorF) {
  return function() { // Eff wrapper
    var p = new Promise(
      function(resolveF, rejectF) {
        var resolveF_eff = function(value) { return resolveF(value)(/*Eff*/); };
        var rejectF_eff = function(error) { return rejectF(error)(/*Eff*/); };
        executorF(resolveF_eff)(rejectF_eff)(/*Eff*/);
      }
    );
    return p;
  };
};


exports.thnImpl = function(onSuccessF, promise) {
  return function() { // Eff wrapper
    var p = promise.then( function(value) { onSuccessF(value)(/*Eff*/); });
    return p;
  };
};


exports.thnPrimeImpl = function(onSuccessF, promise) {
  return function() { // Eff wrapper
    var p = promise.then( function(value) {
      var r = onSuccessF(value)(/*Eff*/);
      return r;
    });

    return p;
  };
};


exports.catchImpl = function(onRejectF, promise) {
  return function() { // Eff wrapper
    var p = promise.catch( function(err) { onRejectF(err)(/*Eff*/); } );
    return p;
  };
};


exports.finallyImpl = function(onFinallyF, promise) {
  return function() { // Eff wrapper
    var p = promise.finally( function() { onFinallyF(/*Eff*/); } );
    return p;
  };
};


exports.mkRejectImpl = function(err) {
  return function() { // Eff wrapper
    var p = Promise.reject( err );
    return p;
  };
};


exports.mkResolveImpl = function(value) {
  return function() { // Eff wrapper
    var p = Promise.resolve( value );
    return p;
  };
};


exports.raceImpl = function(promises) {
  return function() { // Eff wrapper
    var p = Promise.race( promises );
    return p;
  };
};


exports.allImpl = function(promises) {
  return function() { // Eff wrapper
    var p = Promise.all( promises );
    return p;
  };
};
