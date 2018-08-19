'use strict';

// exports._copy = function (m) {
//   var r = {};
//   for (var k in m) {
//     if (hasOwnProperty.call(m, k)) {
//       r[k] = m[k];
//     }
//   }
//   return r;
// };


exports.vnodeTag = function (vnode) {
  return vnode.tag;
};


exports.vnodeKey_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (vnode) {

      if (vnode.key)
        return maybe_just( vnode.key );

      return maybe_nothing;

    };
  };
};


exports.vnodeChildren_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (vnode) {

      if (vnode.children === null || vnode.children === undefined)
        return maybe_nothing;

      if (vnode.children instanceof Array)
        return maybe_just( vnode.children );

      return maybe_nothing;

    };
  };
};


exports.mkVNode_foreign = function (either_isLeft, either_fromLeft, either_fromRight, selector, attrs, childNodes) {
  var selector_ = either_isLeft( selector ) ? either_fromLeft( selector ) : either_fromRight( selector );
  var childNodes_ = either_isLeft( childNodes ) ? either_fromLeft( childNodes ) : either_fromRight( childNodes );
  return m( selector_, attrs, childNodes_ );
};


//---------------------------------------------------------------
// COMPONENT
//---------------------------------------------------------------

exports.mkComponent_foreign = function (newRefF, readRefF, writeRefF, state, viewF) {
  var newComponent = {};

  var stateRef = newRefF( state )(/*Eff*/);
  newComponent.state = stateRef;

  newComponent.view = function(vnode) {
    return viewF( vnode.state.state )( vnode )(/*Eff*/);
  };

  return newComponent;
};


exports.mkComponentVNode_foreign = function (component) {
  return m( component );
};


exports.onInit_foreign = function (unitVal, readRefF, component, initF) {
  var componentCopy = exports._copy( component );
  componentCopy.oninit = function (vnode) {
    initF(vnode.state.state)(readRefF( vnode.state.state )(/*Eff*/))( vnode )(/*Eff*/);
  };
  return componentCopy;
};


exports.onCreate_foreign = function (unitVal, readRefF, component, createF) {
  var componentCopy = exports._copy( component );
  componentCopy.oncreate = function (vnode) {
    createF(vnode.state.state)(readRefF( vnode.state.state )(/*Eff*/))( vnode )(/*Eff*/);
  };
  return componentCopy;
};


exports.onBeforeUpdate_foreign = function (unitVal, readRefF, component, beforeUpdateF) {
  var componentCopy = exports._copy( component );
  componentCopy.onbeforeupdate = function (vnode) {
    beforeUpdateF(vnode.state.state)(readRefF( vnode.state.state )(/*Eff*/))( vnode )(/*Eff*/);
  };
  return componentCopy;
};


exports.onUpdate_foreign = function (unitVal, readRefF, component, updateF) {
  var componentCopy = exports._copy( component );
  componentCopy.onupdate = function (vnode) {
    updateF(vnode.state.state)(readRefF( vnode.state.state )(/*Eff*/))( vnode )(/*Eff*/);
  };
  return componentCopy;
};


exports.onBeforeRemove_foreign = function (unitVal, readRefF, component, beforeRemoveF) {
  var componentCopy = exports._copy( component );
  componentCopy.onbeforeremove = function (vnode) {
    beforeRemoveF(vnode.state.state)(readRefF( vnode.state.state )(/*Eff*/))( vnode )(/*Eff*/);
  };
  return componentCopy;
};


exports.onRemove_foreign = function (unitVal, readRefF, component, removeF) {
  var componentCopy = exports._copy( component );
  componentCopy.onremove = function (vnode) {
    removeF(vnode.state.state)(readRefF( vnode.state.state )(/*Eff*/))( vnode )(/*Eff*/);
  };
  return componentCopy;
};


//---------------------------------------------------------------
// EVENTS
//---------------------------------------------------------------

exports.raise_foreign = function (unitVal, readRefF, writeRefF, updateF, mutableState, msg) {
  updateF( mutableState )( msg )(/*Eff*/);
  return unitVal;
};


//---------------------------------------------------------------
// MITHRIL CORE
//---------------------------------------------------------------

exports.render_foreign = function (unitVal) {
  return function (either_isLeft) {
    return function (either_fromLeft) {
      return function (either_fromRight) {
        return function (elem) {
          return function (vnodes) {

            return function () {
              if (either_isLeft( vnodes )) {
                m.render( elem, either_fromLeft( vnodes ) );
              }
              else {
                m.render( elem, either_fromRight( vnodes ) );
              }

              return unitVal;
            };

          };
        };
      };
    };
  };
};


exports.mount_foreign = function (unitVal, elem, component) {
  return function() { // Eff wrapper
    m.mount( elem, component );
    return unitVal;
  };
};



exports.route_foreign = function (unitVal, rootElem, defaultRoute, routes) {
  return function() { // Eff wrapper
    m.route( rootElem, defaultRoute, routes );
    return unitVal;
  }
};


exports.redraw_foreign = function (unitVal) {
  return function () { // Eff wrapper
    m.redraw();
    return unitVal;
  };
};


exports.parseQueryString = function (str) {
  return m.parseQueryString( str );
};


exports.buildQueryString = function (object) {
  return m.buildQueryString( object );
};


exports.trust = function (html) {
  return m.trust( html );
};


exports.version = m.version;
