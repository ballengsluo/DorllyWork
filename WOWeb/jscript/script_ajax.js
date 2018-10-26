

function createAjaxObj(){      
    var xmlhttp;
    if(window.XMLHttpRequest){
        //如果为 Mozilla，Safari 等浏览器
        xmlhttp=new XMLHttpRequest();
        if(xmlhttp.overrideMimeType)
            xmlhttp.overrideMimeType('text/xml');
    }
    else if(window.ActiveXObject){    //如果是Ie浏览器
        try{
            xmlhttp=new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch(e){
            try{
                xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch(e){}
        }

    }
    return xmlhttp;    
}

function getRootPath(){  
    var curWwwPath=window.document.location.href;    
    var pathName=window.document.location.pathname;    
    var pos=curWwwPath.indexOf(pathName);    
    var localhostPaht=curWwwPath.substring(0,pos);
    var projectName=pathName.substring(0,pathName.substr(1).indexOf('/')+1);  
    return (localhostPaht + projectName.replace("/pm", "").replace("/platform", ""));
}

function ajaxresponse(url){
    var httpRequest=createAjaxObj();
    if(httpRequest)
    {
        httpRequest.open( 'POST', getRootPath()+"/serverpage.aspx?"+url, false ) ;
        httpRequest.setRequestHeader( "Content-Type", "application/x-www-form-urlencoded; charset=utf-8" ) ;
	    httpRequest.send();
	    
        var result = null ;
        if( httpRequest.status == 200 )
                result = httpRequest.responseText ;
        return result ;
	}
}


function ajaxSubmit( url, submitDate )
{
    var httpRequest ;

    if( typeof XMLHttpRequest != 'undefined' )
        httpRequest = new XMLHttpRequest() ;
    else if( typeof ActiveXObject != 'undefined' )
        httpRequest = new ActiveXObject( 'Microsoft.XMLHTTP' ) ;

    if( httpRequest )
    {
        var date = "&date=" + submitDate ;

        httpRequest.open( 'POST', url, false ) ;
        httpRequest.setRequestHeader( "content-length", date.length ) ;
        httpRequest.setRequestHeader( "Content-Type", "application/x-www-form-urlencoded; charset=utf-8" ) ;
        httpRequest.send( date ) ;

        var result = null ;
        if( httpRequest.status == 200 )
            result = httpRequest.responseText.toString() ;
        
        return result ;
    }
}