/* ========================================================
 * bootstrap-tab.js v2.0.4
 * http://twitter.github.com/bootstrap/javascript.html#tabs
 * ========================================================
 * Copyright 2012 Twitter, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Modified: lvxiaoyong
 * ======================================================== */
!function ($) {

    "use strict";

    /* TAB CLASS DEFINITION
    * ==================== */
    var Tab = function (element) {
        this.element = $(element);
    }

    Tab.prototype = {

        constructor: Tab

    , show: function () {
        var $this = this.element,
            $ul = $this.closest('div.nav-tabs'),
            selector = $this.attr('data-target'),
            previous,
            $target,
            e;

        if (!selector) {
            selector = $this.attr('href');
            selector = selector && selector.replace(/.*(?=#[^\s]*$)/, ''); //strip for ie7
        }

        if ($this.hasClass('current')) return;

        previous = $ul.find('.current').last()[0];

        e = $.Event('show', {
            relatedTarget: previous
        });

        $this.trigger(e);

        if (e.isDefaultPrevented()) return;

        $target = $(selector);

        this.activate($this, $ul);
        this.activate($target, $target.parent(), function () {
            $this.trigger({
                type: 'shown',
                relatedTarget: previous
            });
        });
    }

    , activate: function (element, container, callback) {
        var $active = container.find('> .current'),
            transition = callback
                && $.support.transition
                && $active.hasClass('fade');

        function next() {
            $active
                .removeClass('current');

            element.addClass('current');

            if (transition) {
                element[0].offsetWidth // reflow for transition
                element.addClass('in');
            } else {
                element.removeClass('fade');
            }

            if (element.parent('.dropdown-menu')) {
                element.closest('li.dropdown').addClass('active');
            }

            callback && callback();
        }

        transition ?
            $active.one($.support.transition.end, next) :
            next();

        $active.removeClass('in');
    }
    }

    /* TAB PLUGIN DEFINITION
     * ===================== */
    $.fn.tab = function (option) {
        return this.each(function () {
            var $this = $(this), data = $this.data('tab');
            if (!data) $this.data('tab', (data = new Tab(this)));
            if (typeof option == 'string') data[option]();
        });
    }
    $.fn.tab.Constructor = Tab;

    /* TAB DATA-API
     * ============ */
    $(function () {
        $('body').on('click.tab.data-api', '[data-toggle="tab"]', function (e) {
            e.preventDefault();
            $(this).tab('show');
        });
    });
}(window.jQuery);

/* =========================================================
 * bootstrap-modal.js v2.3.2
 * http://getbootstrap.com/2.3.2/javascript.html#modals
 * =========================================================
 * Copyright 2013 Twitter, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ========================================================= */


!function ($) {

    "use strict"; // jshint ;_;


    /* MODAL CLASS DEFINITION
     * ====================== */

    var Modal = function (element, options) {
        this.options = options
        this.$element = $(element)
          .delegate('[data-dismiss="modal"]', 'click.dismiss.modal', $.proxy(this.hide, this))
        this.options.remote && this.$element.find('.modal-body').load(this.options.remote, function() {
            var h = $(element).height();
            $(element).css("margin-top", -(h / 2));
        });
    }

    Modal.prototype = {

        constructor: Modal

      , toggle: function () {
          return this[!this.isShown ? 'show' : 'hide']()
      }

      , show: function () {
          var that = this
            , e = $.Event('show')

          this.$element.trigger(e)

          if (this.isShown || e.isDefaultPrevented()) return

          this.isShown = true

          this.escape()

          this.backdrop(function () {
              var transition = $.support.transition && that.$element.hasClass('fade')

              if (!that.$element.parent().length) {
                  that.$element.appendTo(document.body) //don't move modals dom position
              }

              that.$element.show()

              if (transition) {
                  that.$element[0].offsetWidth // force reflow
              }

              that.$element
                .addClass('in')
                .attr('aria-hidden', false)

              that.enforceFocus()

              transition ?
                that.$element.one($.support.transition.end, function () { that.$element.focus().trigger('shown') }) :
                that.$element.focus().trigger('shown')

          })
      }

      , hide: function (e) {
          e && e.preventDefault()

          var that = this

          e = $.Event('hide')

          this.$element.trigger(e)

          if (!this.isShown || e.isDefaultPrevented()) return

          this.isShown = false

          this.escape()

          $(document).off('focusin.modal')

          this.$element
            .removeClass('in')
            .attr('aria-hidden', true)

          $.support.transition && this.$element.hasClass('fade') ?
            this.hideWithTransition() :
            this.hideModal()
      }

      , enforceFocus: function () {
          var that = this
          $(document).on('focusin.modal', function (e) {
              if (that.$element[0] !== e.target && !that.$element.has(e.target).length) {
                  that.$element.focus()
              }
          })
      }

      , escape: function () {
          var that = this
          if (this.isShown && this.options.keyboard) {
              this.$element.on('keyup.dismiss.modal', function (e) {
                  e.which == 27 && that.hide()
              })
          } else if (!this.isShown) {
              this.$element.off('keyup.dismiss.modal')
          }
      }

      , hideWithTransition: function () {
          var that = this
            , timeout = setTimeout(function () {
                that.$element.off($.support.transition.end)
                that.hideModal()
            }, 500)

          this.$element.one($.support.transition.end, function () {
              clearTimeout(timeout)
              that.hideModal()
          })
      }

      , hideModal: function () {
          var that = this
          this.$element.hide()
          this.backdrop(function () {
              that.removeBackdrop()
              that.$element.trigger('hidden')
          })
      }

      , removeBackdrop: function () {
          this.$backdrop && this.$backdrop.remove()
          this.$backdrop = null
      }

      , backdrop: function (callback) {
          var that = this
            , animate = this.$element.hasClass('fade') ? 'fade' : ''

          if (this.isShown && this.options.backdrop) {
              var doAnimate = $.support.transition && animate

              this.$backdrop = $('<div class="modal-backdrop ' + animate + '" />')
                .appendTo(document.body)

              this.$backdrop.click(
                this.options.backdrop == 'static' ?
                  $.proxy(this.$element[0].focus, this.$element[0])
                : $.proxy(this.hide, this)
              )

              if (doAnimate) this.$backdrop[0].offsetWidth // force reflow

              this.$backdrop.addClass('in')

              if (!callback) return

              doAnimate ?
                this.$backdrop.one($.support.transition.end, callback) :
                callback()

          } else if (!this.isShown && this.$backdrop) {
              this.$backdrop.removeClass('in')

              $.support.transition && this.$element.hasClass('fade') ?
                this.$backdrop.one($.support.transition.end, callback) :
                callback()

          } else if (callback) {
              callback()
          }
      }
    }


    /* MODAL PLUGIN DEFINITION
     * ======================= */

    var old = $.fn.modal

    $.fn.modal = function (option) {
        return this.each(function () {
            var $this = $(this)
              , data = $this.data('modal')
              , options = $.extend({}, $.fn.modal.defaults, $this.data(), typeof option == 'object' && option)
            if (!data) $this.data('modal', (data = new Modal(this, options)))
            if (typeof option == 'string') data[option]()
            else if (options.show) data.show()
        })
    }

    $.fn.modal.defaults = {
        backdrop: true
      , keyboard: true
      , show: true
    }

    $.fn.modal.Constructor = Modal


    /* MODAL NO CONFLICT
     * ================= */

    $.fn.modal.noConflict = function () {
        $.fn.modal = old
        return this
    }


    /* MODAL DATA-API
     * ============== */

    $(document).on('click.modal.data-api', '[data-toggle="modal"]', function (e) {
        var $this = $(this)
          , href = $this.attr('href')
          , $target = $($this.attr('data-target') || (href && href.replace(/.*(?=#[^\s]+$)/, ''))) //strip for ie7
          , option = $target.data('modal') ? 'toggle' : $.extend({ remote: !/#/.test(href) && href }, $target.data(), $this.data())

        e.preventDefault()

        $target
          .modal(option)
          .one('hide', function () {
              $this.focus()
          })
    })

}(window.jQuery);

//  ----------------------------------------------------------------------------
//
//  bootstrap-typeahead.js  
//
//  Twitter Bootstrap Typeahead Plugin
//  v1.2.2
//  https://github.com/tcrosen/twitter-bootstrap-typeahead
//
//
//  Author
//  ----------
//  Terry Rosen
//  tcrosen@gmail.com | @rerrify | github.com/tcrosen/
//
//
//  Description
//  ----------
//  Custom implementation of Twitter's Bootstrap Typeahead Plugin
//  http://twitter.github.com/bootstrap/javascript.html#typeahead
//
//
//  Requirements
//  ----------
//  jQuery 1.7+
//  Twitter Bootstrap 2.0+
//
//  ----------------------------------------------------------------------------

!
function ($) {

    "use strict";

    //------------------------------------------------------------------
    //
    //  Constructor
    //
    var Typeahead = function (element, options) {
        this.$element = $(element);
        this.options = $.extend(true, {}, $.fn.typeahead.defaults, options);
        //this.$menu = $(this.options.menu).appendTo('body');
        this.$menu = this.$element.parent().find("ul");    // xiaoyong.lv 
        this.shown = false;

        // Method overrides    
        this.eventSupported = this.options.eventSupported || this.eventSupported;
        this.grepper = this.options.grepper || this.grepper;
        this.highlighter = this.options.highlighter || this.highlighter;
        this.lookup = this.options.lookup || this.lookup;
        this.matcher = this.options.matcher || this.matcher;
        this.render = this.options.render || this.render;
        this.select = this.options.select || this.select;
        this.sorter = this.options.sorter || this.sorter;
        this.source = this.options.source || this.source;

        if (!this.source.length) {
            var ajax = this.options.ajax;

            if (typeof ajax === 'string') {
                this.ajax = $.extend({}, $.fn.typeahead.defaults.ajax, { url: ajax });
            } else {
                this.ajax = $.extend({}, $.fn.typeahead.defaults.ajax, ajax);
            }

            if (!this.ajax.url) {
                this.ajax = null;
            }
        }

        this.listen();
    }

    Typeahead.prototype = {

        constructor: Typeahead,

        //=============================================================================================================
        //
        //  Utils
        //
        //=============================================================================================================

        //------------------------------------------------------------------
        //
        //  Check if an event is supported by the browser eg. 'keypress'
        //  * This was included to handle the "exhaustive deprecation" of jQuery.browser in jQuery 1.8
        //
        eventSupported: function (eventName) {
            var isSupported = (eventName in this.$element);

            if (!isSupported) {
                this.$element.setAttribute(eventName, 'return;');
                isSupported = typeof this.$element[eventName] === 'function';
            }

            return isSupported;
        },

        //=============================================================================================================
        //
        //  AJAX
        //
        //=============================================================================================================

        //------------------------------------------------------------------
        //
        //  Handle AJAX source 
        //
        ajaxer: function () {
            var that = this,
                query = that.$element.val();

            if (query === that.query) {
                return that;
            }

            // Query changed
            that.query = query;

            // Cancel last timer if set
            if (that.ajax.timerId) {
                clearTimeout(that.ajax.timerId);
                that.ajax.timerId = null;
            }

            if (!query || query.length < that.ajax.triggerLength) {
                // Cancel the ajax callback if in progress
                if (that.ajax.xhr) {
                    that.ajax.xhr.abort();
                    that.ajax.xhr = null;
                    that.ajaxToggleLoadClass(false);
                }

                return that.shown ? that.hide() : that;
            }

            // Query is good to send, set a timer
            that.ajax.timerId = setTimeout(function () {
                $.proxy(that.ajaxExecute(query), that)
            }, that.ajax.timeout);

            return that;
        },

        //------------------------------------------------------------------
        //
        //  Execute an AJAX request
        //
        ajaxExecute: function (query) {
            this.ajaxToggleLoadClass(true);

            // Cancel last call if already in progress
            if (this.ajax.xhr) this.ajax.xhr.abort();

            var params = this.ajax.preDispatch ? this.ajax.preDispatch(query) : { query: query };
            var jAjax = (this.ajax.method === "post") ? $.post : $.get;
            this.ajax.xhr = jAjax(this.ajax.url, params, $.proxy(this.ajaxLookup, this));
            this.ajax.timerId = null;
        },

        //------------------------------------------------------------------
        //
        //  Perform a lookup in the AJAX results
        //
        ajaxLookup: function (data) {
            var items;

            this.ajaxToggleLoadClass(false);

            if (!this.ajax.xhr) return;

            if (this.ajax.preProcess) {
                data = this.ajax.preProcess(data);
            }

            // Save for selection retreival
            this.ajax.data = data;

            items = this.grepper(this.ajax.data);

            if (!items || !items.length) {
                return this.shown ? this.hide() : this;
            }

            this.ajax.xhr = null;

            return this.render(items.slice(0, this.options.items)).show();
        },

        //------------------------------------------------------------------
        //
        //  Toggle the loading class
        //
        ajaxToggleLoadClass: function (enable) {
            if (!this.ajax.loadingClass) return;
            this.$element.toggleClass(this.ajax.loadingClass, enable);
        },

        //=============================================================================================================
        //
        //  Data manipulation
        //
        //=============================================================================================================

        //------------------------------------------------------------------
        //
        //  Search source
        //
        lookup: function (event) {
            var that = this,
                items;

            if (that.ajax) {
                that.ajaxer();
            }
            else {
                that.query = that.$element.val();

                if (!that.query) {
                    return that.shown ? that.hide() : that;
                }

                items = that.grepper(that.source);

                if (!items || !items.length) {
                    return that.shown ? that.hide() : that;
                }

                return that.render(items.slice(0, that.options.items)).show();
            }
        },

        //------------------------------------------------------------------
        //
        //  Filters relevent results 
        //
        grepper: function (data) {
            var that = this,
                items;

            if (data && data.length && !data[0].hasOwnProperty(that.options.display)) {
                return null;
            }

            items = $.grep(data, function (item) {
                return that.matcher(item[that.options.display], item);
            });

            return this.sorter(items);
        },

        //------------------------------------------------------------------
        //
        //  Looks for a match in the source
        //
        matcher: function (item) {
            return ~item.toLowerCase().indexOf(this.query.toLowerCase());
        },

        //------------------------------------------------------------------
        //
        //  Sorts the results
        //
        sorter: function (items) {
            var that = this,
                beginswith = [],
                caseSensitive = [],
                caseInsensitive = [],
                item;

            while (item = items.shift()) {
                if (!item[that.options.display].toLowerCase().indexOf(this.query.toLowerCase())) {
                    beginswith.push(item);
                }
                else if (~item[that.options.display].indexOf(this.query)) {
                    caseSensitive.push(item);
                }
                else {
                    caseInsensitive.push(item);
                }
            }

            return beginswith.concat(caseSensitive, caseInsensitive);
        },

        //=============================================================================================================
        //
        //  DOM manipulation
        //
        //=============================================================================================================

        //------------------------------------------------------------------
        //
        //  Shows the results list
        //
        show: function () {
            var pos = $.extend({}, this.$element.offset(), {
                height: this.$element[0].offsetHeight
            });
            /*
            this.$menu.css({
                top: pos.top + pos.height,
                left: pos.left
            });*/   //  xiaoyong.lv

            this.$menu.show();
            this.shown = true;

            return this;
        },

        //------------------------------------------------------------------
        //
        //  Hides the results list
        //
        hide: function () {
            this.$menu.hide();
            this.shown = false;
            return this;
        },

        //------------------------------------------------------------------
        //
        //  Highlights the match(es) within the results
        //
        highlighter: function (item) {
            var query = this.query.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, '\\$&');
            return item.replace(new RegExp('(' + query + ')', 'ig'), function ($1, match) {
                return '<strong>' + match + '</strong>';
            });
        },

        //------------------------------------------------------------------
        //
        //  Renders the results list
        //
        render: function (items) {
            var that = this;

            items = $(items).map(function (i, item) {
                i = $(that.options.item).attr('data-value', item[that.options.val]);
                i.find('a').html(that.highlighter(item[that.options.display], item));
                return i[0];
            });

            items.first().addClass('active');
            this.$menu.html(items);
            return this;
        },

        //------------------------------------------------------------------
        //
        //  Item is selected
        //
        select: function () {
            var $selectedItem = this.$menu.find('.active');
            this.$element.val($selectedItem.text()).change();
            this.options.itemSelected($selectedItem, $selectedItem.attr('data-value'), $selectedItem.text());
            return this.hide();
        },

        //------------------------------------------------------------------
        //
        //  Selects the next result
        //
        next: function (event) {
            var active = this.$menu.find('.active').removeClass('active');
            var next = active.next();

            if (!next.length) {
                next = $(this.$menu.find('li')[0]);
            }

            next.addClass('active');
        },

        //------------------------------------------------------------------
        //
        //  Selects the previous result
        //
        prev: function (event) {
            var active = this.$menu.find('.active').removeClass('active');
            var prev = active.prev();

            if (!prev.length) {
                prev = this.$menu.find('li').last();
            }

            prev.addClass('active');
        },

        //=============================================================================================================
        //
        //  Events
        //
        //=============================================================================================================

        //------------------------------------------------------------------
        //
        //  Listens for user events
        //
        listen: function () {
            this.$element.on('blur', $.proxy(this.blur, this))
                         .on('keyup', $.proxy(this.keyup, this));

            if (this.eventSupported('keydown')) {
                this.$element.on('keydown', $.proxy(this.keypress, this));
            } else {
                this.$element.on('keypress', $.proxy(this.keypress, this));
            }

            this.$menu.on('click', $.proxy(this.click, this))
                      .on('mouseenter', 'li', $.proxy(this.mouseenter, this));
        },

        //------------------------------------------------------------------
        //
        //  Handles a key being raised up
        //
        keyup: function (e) {
            e.stopPropagation();
            e.preventDefault();

            switch (e.keyCode) {
                case 40:
                    // down arrow
                case 38:
                    // up arrow
                    break;
                case 9:
                    // tab
                case 13:
                    // enter
                    if (!this.shown) {
                        return;
                    }
                    this.select();
                    break;
                case 27:
                    // escape
                    this.hide();
                    break;
                default:
                    this.lookup();
            }
        },

        //------------------------------------------------------------------
        //
        //  Handles a key being pressed
        //
        keypress: function (e) {
            e.stopPropagation();
            if (!this.shown) {
                return;
            }

            switch (e.keyCode) {
                case 9:
                    // tab
                case 13:
                    // enter
                case 27:
                    // escape
                    e.preventDefault();
                    break;
                case 38:
                    // up arrow
                    e.preventDefault();
                    this.prev();
                    break;
                case 40:
                    // down arrow
                    e.preventDefault();
                    this.next();
                    break;
            }
        },

        //------------------------------------------------------------------
        //
        //  Handles cursor exiting the textbox
        //
        blur: function (e) {
            var that = this;
            e.stopPropagation();
            e.preventDefault();
            setTimeout(function () {
                if (!that.$menu.is(':focus')) {
                    that.hide();
                }
            }, 150)
        },

        //------------------------------------------------------------------
        //
        //  Handles clicking on the results list
        //
        click: function (e) {
            e.stopPropagation();
            e.preventDefault();
            this.select();
        },

        //------------------------------------------------------------------
        //
        //  Handles the mouse entering the results list
        //
        mouseenter: function (e) {
            this.$menu.find('.active').removeClass('active');
            $(e.currentTarget).addClass('active');
        }
    }

    //------------------------------------------------------------------
    //
    //  Plugin definition
    //
    $.fn.typeahead = function (option) {
        return this.each(function () {
            var $this = $(this),
                data = $this.data('typeahead'),
                options = typeof option === 'object' && option;

            if (!data) {
                $this.data('typeahead', (data = new Typeahead(this, options)));
            }

            if (typeof option === 'string') {
                data[option]();
            }
        });
    }

    //------------------------------------------------------------------
    //
    //  Defaults
    //
    $.fn.typeahead.defaults = {
        source: [],
        items: 8,
        menu: '<ul class="typeahead dropdown-menu"></ul>',
        item: '<li><a href="#"></a></li>',
        display: 'name',
        val: 'id',
        itemSelected: function () { },
        ajax: {
            url: null,
            timeout: 300,
            method: 'post',
            triggerLength: 3,
            loadingClass: null,
            displayField: null,
            preDispatch: null,
            preProcess: null
        }
    }

    $.fn.typeahead.Constructor = Typeahead;

    //------------------------------------------------------------------
    //
    //  DOM-ready call for the Data API (no-JS implementation)
    //    
    //  Note: As of Bootstrap v2.0 this feature may be disabled using $('body').off('.data-api')    
    //  More info here: https://github.com/twitter/bootstrap/tree/master/js
    //
    $(function () {
        $('body').on('focus.typeahead.data-api', '[data-provide="typeahead"]', function (e) {
            var $this = $(this);

            if ($this.data('typeahead')) {
                return;
            }

            e.preventDefault();
            $this.typeahead($this.data());
        })
    });

}(window.jQuery);
