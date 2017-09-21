function isIE8() {
	return jQuery.browser.msie && jQuery.browser.version == '8.0';
}

// In case we forget to take out console statements. IE becomes very unhappy when we forget. Let's not make IE unhappy
if(typeof(console) === 'undefined') {
    var console = {}
    console.log = console.error = console.info = console.debug = console.warn = console.trace = console.dir = console.dirxml = console.group = console.groupEnd = console.time = console.timeEnd = console.assert = console.profile = function() {};
}

(function($) {          
jQuery( document ).ready( function( $ ) {
	$('body').removeClass('no_js').addClass('yes_js');
	$('#topbar .span6').each(function(i){
		$(this).removeClass('span6').addClass('span'+ ( (i==0) ? '9' : '3' ));
	});
    
    if ( isIE8() ) {
        $('*:last-child').addClass('last-child');
    }
	
	//iPad, iPhone hack
	$('.ch-item').bind('hover', function(e){});
    
    //Form fields shadow
    $( 'form input[type="text"], form textarea' ).focus(function() {
        if( $( this ).hasClass( 'error' ) ) {
            $( this ).addClass( 'formRed formShadowRed' );
        } else {
        	if($(this).attr('id') != 'comment')
            	$( this ).addClass( 'formBlue' );
        }
        
        //Hide label
        $( this ).parent().find( 'label' ).hide(); 
    });
    
    $( 'form input[type="text"], form textarea' ).blur(function() {
        $( this ).removeClass( 'formBlue formGrey formShadowRed' );
        
        //Show label
        if( $( this ).val() == '' )
            { $( this ).parent().find( 'label' ).show(); }
    });
    
    $( 'form input[type="text"], form textarea' ).each( function() {
        //Show label
        if( $( this ).val() != '' && $( this ).parent().find( 'label' ).is( ':visible' ) )
            { $( this ).parent().find( 'label' ).hide(); }
    } );
    
    //Contact form labels
    $( '.contact-form [type="text"], .contact-form textarea' ).focus( function() {
        //Hide label
        $( this ).parents( 'li' ).find( 'label' ).hide(); 
    } );
    
    $( '.contact-form [type="text"], .contact-form textarea' ).blur( function() {
        //Show label
        if( $( this ).val() == '' )
            { $( this ).parents( 'li' ).find( 'label' ).show(); }
    } );
    
    //Sticky Footer
    if( $( '#footer' ).length )
        { $( '#footer' ).stickyFooter(); }
    else
        { $( '#copyright' ).stickyFooter(); }
    
    //Map handler
    $( '#map-handler a' ).click( function() {
        $( '#map iframe' ).slideToggle( 400, function() {
            if( $( '#map iframe' ).is( ':visible' ) ) {
                $( '#map-handler a' ).text( l10n_handler.map_close );
            } else {
                $( '#map-handler a' ).text( l10n_handler.map_open );
            }
        });
    } );
	
	menuSpan();
	
    $("ul.sf-menu").superfish({ 
		autoArrows: true,
		animation: {opacity:'show', height:'show'},
        speed: 'fast',
        delay: 200,
        onBeforeShow: function(){
        	var level = $(this).parents('ul').length;
			var main_container_width = $('body').width();
			var parent = $(this).parents('ul.sf-menu > li');
			var megamenu = $(this);

			var width_megamenu = megamenu.outerWidth();
			var position_megamenu = megamenu.offset();
			var position_parent = parent.length > 0 && parent.position().left != undefined ? parent.position().left : 0;
			var position_right_megamenu = level <= 1 ? position_parent + width_megamenu : position_parent + width_megamenu + 800;
			
			var offset_parent = parent.length > 0 ? parent.offset().left : 0;

			//console.log(parent, position_parent, position_right_megamenu, position_right_megamenu > main_container_width)
			if ( position_right_megamenu > main_container_width ) {
				//console.log(  (width_megamenu - (main_container_width - offset_parent) + 10) + 'px');
				megamenu.data("left", $(this).css('left') )
						.css('left', '-' + (width_megamenu - (main_container_width - offset_parent) + 10) + 'px');
			}
        },
        onHide: function() {
        	if( $(this).data('left') ) {
        		$(this).css('left', $(this).data('left'));
        	}
        }
	});//.find('li:nth-child(7)').nextAll('li').hide();
	
	// remove margin from the slider, if the page is empty
	if ( $('.slider').length != 0 && $.trim( $('#primary *, #page-meta').text() ) == '' ) {
        $('.slider').attr('style', 'margin-bottom:0 !important;');
        $('#primary').remove();
    }
	
/*	
    // menu in responsive, with select
    if( $('body').hasClass('responsive') ) {  
        $('#nav').parent().after('<div class="menu-select"></div>');
        $('#nav').clone().appendTo('.menu-select');  
        $('.menu-select #nav').attr('id', 'nav-select').after('<div class="arrow-icon"></div>');
                  
        $( '#nav-select' ).hide().mobileMenu({
            subMenuDash : '-'
        });
//         
//         $( '#nav > ul, #nav .menu > ul' ).hide();
    }
*/
   
    //play, zoom on image hover
	var yit_lightbox;
	(yit_lightbox = function(){
	    jQuery('a.thumb.video, a.thumb.img, img.thumb.img, img.thumb.project, .work-thumbnail a, .three-columns li a, .onlytitle, .overlay_a img, .nozoom img').hover(
	        function()
	        {
	            jQuery(this).next('.overlay').fadeIn(500);
	            jQuery(this).next('.overlay').children('.lightbox, .details, .lightbox-video').animate({
	                bottom:'40%'
	            }, 300);
	            
	            jQuery(this).next('.overlay').children('.title').animate({
	                top:'30%'
	            }, 300);
	        }
	    );
	    
	    /*
	    jQuery( '.overlay' ).hover(
	        function() {},
	        function()
	        {
	            jQuery(this).children('.lightbox, .details, .lightbox-video').animate({
	                bottom:'0%'
	            }, 300);
	            jQuery(this).children('.title').animate({
	                top:'0%'
	            }, 300);
	            jQuery(this).fadeOut(300);
	        }
	    );
	    */
	    
	    // image lightbox
	    $('a.thumb.video, a.thumb.img, a.thumb.videos, a.thumb.imgs, a.related_detail, a.related_proj, a.related_video, a.related_title, a.project, a.onlytitle').hover(function(){
	        $('<a class="zoom"></a>').appendTo(this).css({
	            dispay:'block', 
	            opacity:0, 
	            height:$(this).children('img').height(), 
	            width:$(this).children('img').width(),
	            'top': $(this).parents('.portfolio-filterable').length ? '-1px' : $(this).css('padding-top'),
	            'left':$(this).parents('.portfolio-filterable').length ? '-1px' : $(this).css('padding-left'),
	            padding:0}).append('<span class="title">' + $(this).attr('title') + '</span>')
	            		   .append('<span class="subtitle">' + $(this).attr('data-subtitle') + '</span>').animate({opacity:0.7}, 500);
	            
	            //if ( $(this).attr('title') != undefined )    $('.zoom', this).append('<span class="title">' + $(this).attr('title') + '</span>');
	            //if ( $(this).attr('subtitle') != undefined ) $('.zoom', this).append('<span class="subtitle">' + $(this).attr('data-subtitle') + '</span>').animate({opacity:0.7}, 500);
	        },        
	        function(){           
	            $('.zoom').fadeOut(500, function(){$(this).remove()});
	        }
	    );
	    
	    $('.zoom').live('click', function(){
	    	if( $.browser.msie ) {
	    		$(this).attr('href', $(this).parent().attr('href'));
	    	}
	    });
	    
	    if( jQuery( 'body' ).hasClass( 'isMobile' ) ) {
	        jQuery("a.thumb.img, .overlay_img, .section .related_proj, a.ch-info-lightbox").colorbox({
	            transition:'elastic',
	            rel:'lightbox',
	    		fixed:true,
	    		maxWidth: '100%',
	    		maxHeight: '100%',
	    		opacity : 0.7
	        });
	        
	        jQuery(".section .related_lightbox").colorbox({
	            transition:'elastic',
	            rel:'lightbox2',
	    		fixed:true,
	    		maxWidth: '100%',
	    		maxHeight: '100%',
	    		opacity : 0.7
	        });
	    } else {
	        jQuery("a.thumb.img, .overlay_img, .section.portfolio .related_proj, a.ch-info-lightbox, a.ch-info-lightbox").colorbox({
	            transition:'elastic',
	            rel:'lightbox',
	    		fixed:true,
	    		maxWidth: '80%',
	    		maxHeight: '80%',
	    		opacity : 0.7
	        });   
	        
	        jQuery(".section.portfolio .related_lightbox").colorbox({
	            transition:'elastic',
	            rel:'lightbox2',
	    		fixed:true,
	    		maxWidth: '80%',
	    		maxHeight: '80%',
	    		opacity : 0.7
	        });   
	    }
	    
	    jQuery("a.thumb.video, .overlay_video, .section.portfolio .related_video, a.ch-info-lightbox-video").colorbox({
	        transition:'elastic',
	        rel:'lightbox',
			fixed:true,
			maxWidth: '60%',
			maxHeight: '80%',
	        innerWidth: '60%',
	        innerHeight: '80%',
			opacity : 0.7,
	        iframe: true,
	        onOpen: function() { $( '#cBoxContent' ).css({ "-webkit-overflow-scrolling": "touch" }) }
	    });
	    jQuery(".section.portfolio .lightbox_related_video").colorbox({
	        transition:'elastic',
	        rel:'lightbox2',
			fixed:true,
			maxWidth: '60%',
			maxHeight: '80%',
	        innerWidth: '60%',
	        innerHeight: '80%',
			opacity : 0.7,
	        iframe: true,
	        onOpen: function() { $( '#cBoxContent' ).css({ "-webkit-overflow-scrolling": "touch" }) }
	    });
	})();      
            
              
    //FILTERABLE
    if( $('.portfolio-filterable').length > 0 ) {
    	$('.gallery-categories-disabled, .portfolio-categories-disabled').addClass('gallery-categories-quicksand');
    }
    
    
    $(".gallery-wrap .internal_page_item .overlay, .section .related_project .overlay").css({opacity:0});
	$(".gallery-wrap .internal_page_item, .section .related_project > div").live( 'mouseover mouseout', function(event){ 
		if ( event.type == 'mouseover' ) $('.overlay', this).show().stop(true,false).animate({ opacity: .7 }, "fast"); 
		if ( event.type == 'mouseout' )  $('.overlay', this).animate({ opacity: 0 }, "fast", function(){ $(this).hide() }); 
	});
	
    var read_button = function(class_names) {
        
        var r = {
            selected: false,
            type: 0
        };
        
        for (var i=0; i < class_names.length; i++) {
            
            if (class_names[i].indexOf('selected-') == 0) {
                r.selected = true;
            }
        
            if (class_names[i].indexOf('segment-') == 0) {
                r.segment = class_names[i].split('-')[1];
            }
        };
        
        return r;
        
    };



  	$('.picture_overlay').hover(function(){
  		
	  	var width = $(this).find('.overlay div').innerWidth();
	  	var height =  $(this).find('.overlay div').innerHeight();
	  	var div = $(this).find('.overlay div').css({
	  		'margin-top' : - height / 2,
	  		'margin-left' : - width / 2
	  	});

  		//$(this).find('.overlay').stop(true,true).fadeTo('normal', 1);//), 'easeInOutBounce', function(){})
  		
		if(isIE8()) {
  			$(this).find('.overlay > div').show();
  		}
  	}, function(){
  		//$(this).find('.overlay').stop(true,true).fadeTo('normal', 0);//, 'easeInOutBounce', function(){})
  		
  		if(isIE8()) {
  			$(this).find('.overlay > div').hide();
  		}
  	}).each(function(){
	  	var width = $(this).find('.overlay div').innerWidth();
	  	var height =  $(this).find('.overlay div').innerHeight();
	  	var div = $(this).find('.overlay div').css({
	  		'margin-top' : - height / 2,
	  		'margin-left' : - width / 2
	  	});
	});
	
	$(window).resize(function(){
		menuSpan();
	});


	//masonry pinterest blog style
	//if ( $.masonry ) {
    var container = $('#pinterest-container');
    container.imagesLoaded( function(){
    	container.masonry({
    	  itemSelector: '.post'
    	});
    });
    	
    $(window).resize(function(){
    	$('#pinterest-container').masonry({
    	  itemSelector: '.post'
    	});
    });
    //}



/*    
//ipad and iphone fix
if((navigator.userAgent.match(/iPhone/i)) || (navigator.userAgent.match(/iPod/i)) ||   (navigator.userAgent.match(/iPad/i))) {
	$('#nav > li > a').click(function(){
		if( !$(this).hasClass('clickable') ) {
			$(this).addClass('clickable');
			return false;			
		}
	});
}
*/

});

                           
function menuSpan() {
	var width = $('body').width();
	if( width <= 979 ) {
		$('#logo').removeClass('span4').addClass('span3');
		$('#menu').removeClass('span8').addClass('span9');
	} else {
		$('#logo').removeClass('span3').addClass('span4');
		$('#menu').removeClass('span9').addClass('span8');		
	}
}


})(jQuery);

// sticky footer plugin
(function($){
  var footer;
 
  $.fn.extend({
    stickyFooter: function(options) {
      footer = this;
       
      positionFooter();
 
      $(window)
        .scroll(positionFooter)
        .resize(positionFooter);
 
      function positionFooter() {
        var docHeight = $(document.body).height() - $("#sticky-footer-push").height();
        
        if(docHeight < $(window).height()){
          var diff = $(window).height() - docHeight;
          if (!$("#sticky-footer-push").length > 0) {
            $(footer).before('<div id="sticky-footer-push"></div>');
          }
          
          if( $('#wpadminbar').length > 0 ) {
            diff -= 28;
          }
          $("#sticky-footer-push").height(diff);
        }
      }
    }
  });
})(jQuery);


//from Celestino