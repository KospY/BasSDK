//	Copyright (c) 2012 Calvin Rien
//        http://the.darktable.com
//
//	This software is provided 'as-is', without any express or implied warranty. In
//	no event will the authors be held liable for any damages arising from the use
//	of this software.
//
//	Permission is granted to anyone to use this software for any purpose,
//	including commercial applications, and to alter it and redistribute it freely,
//	subject to the following restrictions:
//
//	1. The origin of this software must not be misrepresented; you must not claim
//	that you wrote the original software. If you use this software in a product,
//	an acknowledgment in the product documentation would be appreciated but is not
//	required.
//
//	2. Altered source versions must be plainly marked as such, and must not be
//	misrepresented as being the original software.
//
//	3. This notice may not be removed or altered from any source distribution.
//
//  =============================================================================
//
//  derived from Gregorio Zanon's script
//  http://forum.unity3d.com/threads/119295-Writing-AudioListener.GetOutputData-to-wav-problem?p=806734&viewfull=1#post806734

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SavWav
{

    const int HEADER_SIZE = 44;

    public static bool Save(string filename, AudioClip clip)
    {

        return true; // TODO: return false if there's a failure saving the file
    }


    static FileStream CreateEmpty(string filepath)
    {
return default;
    }

    static void ConvertAndWrite(FileStream fileStream, AudioClip clip)
    {
    }

    static void WriteHeader(FileStream fileStream, AudioClip clip)
    {
    }
}