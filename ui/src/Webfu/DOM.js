'use strict';

/* things that return an Eff value in purscript needs to wrap the value in a function.
 *
 */


exports.nodeChildNodes = function (node) {
  var arr = [];
  for (var i = 0; i < node.childNodes.length; i++) {
    arr.push( node.childNodes[i] );
  }

  return arr;
};


exports.nodeFirstChild_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (node) {

        if (node.firstChild)
          return maybe_just( node.firstChild );

        return maybe_nothing;

    };
  };
};


exports.nodeLastChild_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (node) {

        if (node.lastChild)
          return maybe_just( node.lastChild );

        return maybe_nothing;

    };
  };
};


exports.nodeNextSibling_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (node) {

        if (node.nextSibling)
          return maybe_just( node.nextSibling );

        return maybe_nothing;

    };
  };
};


exports.nodePreviousSibling_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (node) {

        if (node.previousSibling)
          return maybe_just( node.previousSibling );

        return maybe_nothing;

    };
  };
};


exports.nodeName = function (node) {
  return node.nodeName;
};


exports.nodeType = function (node) {
  return node.nodeType;
};


exports.nodeValue_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (node) {

        if (node.nodeValue)
          return maybe_just( node.nodeValue );

        return maybe_nothing;

    };
  };
};


exports.nodeOwnerDocument_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (node) {

        if (node.ownerDocument)
          return maybe_just( node.ownerDocument );

        return maybe_nothing;

    };
  };
};


exports.nodeParentNode_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (node) {

        if (node.parentNode)
          return maybe_just( node.parentNode );

        return maybe_nothing;

    };
  };
};


exports.nodeParentElement_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (node) {

        if (node.parentElement)
          return maybe_just( node.parentElement );

        return maybe_nothing;

    };
  };
};


exports.window = function () {
  return window;
};


exports.win_alert_foreign = function (unit_val) {
  return function (msg) {
    return function (w) {

      return function () { // Eff wrapper
        w.alert( msg );
        return unit_val;
      };

    };
  };
};


exports.document = function () {
  return document;
};


exports.doc_getElementById_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (id) {

      return function () {
        var e = document.getElementById( id );
        if (e)
          return maybe_just( e );

        return maybe_nothing;
      };

    };
  };
};


exports.doc_querySelector_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (id) {

      return function () {
        var e = document.querySelector( id );
        if (e)
          return maybe_just( e );

        return maybe_nothing;
      };

    };
  };
};


exports.elemId_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (elem) {

      if (elem.id)
        return maybe_just( elem.id );

      return maybe_nothing;

    };
  };
};


exports.elem_attr_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (attrName) {
      return function (elem) {

        var attrValue = elem.getAttribute( attrName )
        if (attrValue)
          return maybe_just( attrValue );

        return maybe_nothing;

      };
    };
  };
};


exports.el_setAttr_foreign = function (unitVal) {
  return function (attrName) {
    return function (attrVal) {
      return function (elem) {

        return function() { // Eff
          elem.setAttribute( attrName, attrVal )
          return unitVal;
        }

      };
    };
  };
};


exports.elem_prop_foreign = function (maybe_nothing) {
  return function (maybe_just) {
    return function (propName) {
      return function (elem) {

        var propValue = elem[ propName ];

        if (propValue === undefined || propValue === null)
          return maybe_nothing;

        return maybe_just( propValue );

      };
    };
  };
};
